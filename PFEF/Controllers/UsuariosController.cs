using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PFEF.Models;
using PFEF.Models.ViewModels;
using PFEF.Extensions;
using AutoMapper;
using EntityFramework.Extensions;
using PFEF.Models.DataAccess;

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
                var lastvs = ContenidosDA.Recomendaciones.ObtenerIntereses(IUser.Id);
                model.DictRecomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(lastvs);
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
            MappedModel.setDropEsc();
            return View("LlenarPerfil", MappedModel);
        }

        public PartialViewResult GetRecomendaciones()
        {
            Usuarios IUser = User.Identity.GetUserInfoId();
            PerfilViewModel model = MapperPerfilInfo(IUser);
            var lastvs = ContenidosDA.Recomendaciones.ObtenerIntereses(IUser.Id);
            model.DictRecomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(lastvs);
            return PartialView("_Recommendation", model);
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
        #endregion
    }
}
