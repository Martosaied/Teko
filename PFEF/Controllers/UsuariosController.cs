using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PFEF.Models;
using PFEF.Models.ViewModels;
using PFEF.Extensions;
using AutoMapper;
using Newtonsoft.Json;

namespace PFEF.Controllers
{
    public class UsuariosController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.GetUserInfoId().Nombre != null)
            {
                Usuarios IUser = User.Identity.GetUserInfoId();
                PerfilViewModel model = MapperPerfilInfo(IUser);
                string[] lastvs = ObtenerIntereses(IUser.Id);
                model.DictRecomendaciones = ObtenerRec(lastvs);
                return View("HomeUsuario",model);
            }
            else
            {
                Usuarios model = User.Identity.GetUserInfoId();
                InfoUsuarioViewModel MappedModel = MapperUserInfo(model);
                MappedModel.dropEscuela = db.Escuelas.ToList();
                return View("LlenarPerfil", MappedModel);
            }
        }
        [HttpGet]
        public ActionResult LlenarPerfil()
        {
            Usuarios model = User.Identity.GetUserInfoId();
            InfoUsuarioViewModel MappedModel = MapperUserInfo(model);
            MappedModel.dropEscuela = db.Escuelas.ToList();
            return View("LlenarPerfil", MappedModel);
        }

        [HttpPost]
         public ActionResult LlenarPerfil(InfoUsuarioViewModel model, HttpPostedFileBase file)
        {
                    file.SaveAs(Server.MapPath("~/Content/ProfilePH/" + file.FileName));
                    model.RutaFoto = file.FileName;
                    Usuarios MappedUser = MapperUserInfo(model);
                    db.Entry(MappedUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
        }


        #region Helpers
        protected InfoUsuarioViewModel MapperUserInfo(Usuarios model)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Usuarios, InfoUsuarioViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var MappedModel = mapper.Map<Usuarios, InfoUsuarioViewModel>(model);
            return MappedModel;
        }
        protected PerfilViewModel MapperPerfilInfo(Usuarios model)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Usuarios, PerfilViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var MappedModel = mapper.Map<Usuarios, PerfilViewModel>(model);
            return MappedModel;
        }
        protected Usuarios MapperUserInfo(InfoUsuarioViewModel model)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<InfoUsuarioViewModel, Usuarios>();
            });
            IMapper mapper = config.CreateMapper();
            var MappedModel = mapper.Map<InfoUsuarioViewModel, Usuarios>(model);
            return MappedModel;
        }
        protected string[] ObtenerIntereses(int Id)
        {
            string[] Intereses = new string[3];
            var IUser = User.Identity.GetUserInfoId();

            Intereses[1] = db.InteresesEscuelas.Where(x => x.Contador == db.InteresesEscuelas.Max(s => s.Contador) && x.IdUsuario.Id == IUser.Id).Select(x => x.IdEscuela.Id).FirstOrDefault().ToString();
            Intereses[0] = db.InteresesMaterias.Where(x => x.Contador == db.InteresesMaterias.Max(s => s.Contador) && x.IdUsuario.Id == IUser.Id).Select(x => x.IdMateria.Id).FirstOrDefault().ToString();
            Intereses[2] = db.InteresesProfesores.Where(x => x.Contador == db.InteresesProfesores.Max(s => s.Contador) && x.IdUsuario.Id == IUser.Id).Select(x => x.Profesor).FirstOrDefault();
            /*var deserializedDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(IUser.InteresesMaterias);
            var ordered = deserializedDict.OrderByDescending(x => x.Value);
            Intereses[0] = ordered.Select(w => w.Key).First();

            deserializedDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(IUser.InteresesEscuelas);
            ordered = deserializedDict.OrderByDescending(x => x.Value);
            Intereses[1] = ordered.Select(w => w.Key).First();

            deserializedDict = JsonConvert.DeserializeObject<Dictionary<string, int>>(IUser.InteresesProfesores);
            ordered = deserializedDict.OrderByDescending(x => x.Value);
            Intereses[2] = ordered.Select(w => w.Key).First();*/

            return Intereses;
        }
        protected Contenidos[] ObtenerRec(string[] Intereses)
        {
            int mat = Convert.ToInt32(Intereses[0]);
            var esc = Convert.ToInt32(Intereses[1]);
            var prf = Intereses[2];

            var query = db.Contenidos.Where(x => x.Materias.Id == mat
                                     || x.Escuelas.Id == esc
                                     || x.Profesor.Contains(prf))
                                     .ToArray();

            var ListaCoincideTodo = query.Where(x => x.Materias.Id == mat)
                                         .Where(x => x.Escuelas.Id == esc)
                                         .Where(x => x.Profesor.Contains(prf))
                                         .ToArray();

            var ListaCoincideParcial = query.Where(x => x.Escuelas.Id == esc
                                            && x.Materias.Id == mat
                                            || x.Materias.Id == mat
                                            && x.Profesor.Contains(prf)
                                            || x.Escuelas.Id == esc
                                            && x.Profesor.Contains(prf)).ToArray();

            var ListaCoincideUna = query.Where(x => x.Materias.Id == mat
                                           || x.Escuelas.Id == esc
                                           || x.Profesor.Contains(prf)).ToArray();

            var ListaTodo = ListaCoincideTodo.Union(ListaCoincideParcial.Union(ListaCoincideUna)).ToArray();
            return ListaTodo;

            //Esto esta todo mal pero funciona(mal optimizado y trae cosas innecesiaras, ADEMAS DE QUE HACE DEMASIADAS LLAMADAS AL SERVIDOR)
        }
        #endregion
    }
}
