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
using Microsoft.AspNet.Identity;

namespace PFEF.Controllers
{
    public class UsuariosController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var MiUser = HelpersExtensions.ObtenerUser(User.Identity.GetUserId());
            if (MiUser.PerfilCompleto)
            {
                PerfilViewModel model = AutoMapperGeneric<Usuarios, PerfilViewModel>.ConvertToDBEntity(MiUser);
                var lastvs = ContenidosDA.Recomendaciones.ObtenerIntereses(MiUser.Id);
                model.DictRecomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(lastvs);
                return View("HomeUsuario",model);
            }
            else
            {
                InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(MiUser);
                MappedModel.dropEscuela = db.Escuelas.ToList();
                return View("LlenarPerfil", MappedModel);
            }
        }
        [HttpGet]
        public ActionResult LlenarPerfil()
        {
            Usuarios model = HelpersExtensions.ObtenerUser(User.Identity.GetUserId());
            InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(model);
            MappedModel.setDropEsc();
            return View("LlenarPerfil", MappedModel);
        }

        public PartialViewResult GetRecomendaciones()
        {
            Usuarios IUser = HelpersExtensions.ObtenerUser(User.Identity.GetUserId());
            PerfilViewModel model = AutoMapperGeneric<Usuarios,PerfilViewModel>.ConvertToDBEntity(IUser);
            var lastvs = ContenidosDA.Recomendaciones.ObtenerIntereses(IUser.Id);
            model.DictRecomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(lastvs);
            return PartialView("_Recommendation", model);
        }

        [HttpPost]
         public ActionResult LlenarPerfil(InfoUsuarioViewModel model, HttpPostedFileBase file)
        {          
            file.SaveAs(Server.MapPath("~/Content/ProfilePH/" + file.FileName));
            model.RutaFoto = file.FileName;
            Usuarios MappedUser = AutoMapperGeneric<InfoUsuarioViewModel,Usuarios>.ConvertToDBEntity(model);
            MappedUser.PerfilCompleto = true;
            db.Entry(MappedUser).State = EntityState.Modified;
            db.SaveChanges();
            HelpersExtensions.db.Dispose();
                    return RedirectToAction("Index");
        }

    }
}
