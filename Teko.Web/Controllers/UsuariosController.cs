using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Models;
using Teko.Models.ViewModels;
using Teko.Extensions;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Teko.Model;
using Teko.Service;

namespace Teko.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService usuarioService;
        private readonly IContenidoService contenidoService;
        private readonly IVisitaService visitaService;
        private readonly IEscuelaService escuelaService;


        public UsuariosController(IUsuarioService usuarioService, IEscuelaService escuelaService,IContenidoService contenidoService, IVisitaService visitaService)
        {
            this.usuarioService = usuarioService;
            this.contenidoService = contenidoService;
            this.visitaService = visitaService;
            this.escuelaService = escuelaService;
        }
        public ActionResult Index()
        {
            var MiUser = usuarioService.GetById(User.Identity.GetUserId());
            if (MiUser.PerfilCompleto)
            {
                PerfilViewModel model = AutoMapperGeneric<Usuarios, PerfilViewModel>.ConvertToDBEntity(MiUser);
                var lastvs = visitaService.ObtenerIntereses(MiUser.Id);
                return View("HomeUsuario",model);
            }
            else
            {
                InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(MiUser);
                MappedModel.dropEscuela = escuelaService.GetAll();
                return View("LlenarPerfil", MappedModel);
            }
        }
        [HttpGet]
        public ActionResult LlenarPerfil()
        {
            Usuarios model = usuarioService.GetById(User.Identity.GetUserId());
            InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(model);
            MappedModel.dropEscuela = escuelaService.GetAll();
            return View("LlenarPerfil", MappedModel);
        }

        public PartialViewResult GetRecomendaciones()
        {
            Usuarios IUser = usuarioService.GetById(User.Identity.GetUserId());
            PerfilViewModel model = AutoMapperGeneric<Usuarios,PerfilViewModel>.ConvertToDBEntity(IUser);
            var lastvs = visitaService.ObtenerIntereses(IUser.Id);
            model.DictRecomendaciones = contenidoService.ObtenerRecByUser(lastvs);
            return PartialView("_Recommendation", model);
        }

        [HttpPost]
         public ActionResult LlenarPerfil(InfoUsuarioViewModel model, HttpPostedFileBase file)
        {
            var MiUser = usuarioService.GetById(User.Identity.GetUserId());
            file.SaveAs(Server.MapPath("~/Content/ProfilePH/" + file.FileName));
            MiUser.RutaFoto = file.FileName;
            MiUser.Descripcion = model.Descripcion;
            MiUser.InstitucionActualId = model.InstitucionActualId;
            MiUser.Nombre = model.Nombre;
            MiUser.Apellido = model.Apellido;
            MiUser.PerfilCompleto = true;
                usuarioService.Modificar(MiUser);
                usuarioService.SaveUser();
                return RedirectToAction("Index");

        }

    }
}
