using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Extensions;
using Teko.Model;
using Teko.Service;
using Teko.Web.ViewModels;

namespace Teko.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContenidoService contenidoService;
        private readonly IArchivoService archivoService;
        private readonly IEscuelaService escuelaService;
        private readonly IMateriaService materiaService;
        private readonly IVisitaService visitaService;
        private readonly IUsuarioService usuarioService;

        public HomeController(IMateriaService materiaService,IContenidoService contenidoService, IArchivoService archivoService, IEscuelaService escuelaService, IVisitaService visitaService, IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
            this.contenidoService = contenidoService;
            this.archivoService = archivoService;
            this.escuelaService = escuelaService;
            this.visitaService = visitaService;
            this.materiaService = materiaService;
        }

        public HomeController()
        {

        }
        public ActionResult Index()
        {
            Session["Page"] = 0;
            IndexViewModel _indexViewModel = new IndexViewModel(contenidoService,escuelaService);
            var UsuarioId = User.Identity.GetUserId();
            Usuarios UsuarioActual = usuarioService.GetUserById(UsuarioId);
            if (Request.IsAuthenticated && UsuarioActual.PerfilCompleto)
            {
                int EscuelaId = UsuarioActual.InstitucionActual.Id;
                _indexViewModel.setListaArticulosEsc(EscuelaId);
                _indexViewModel.setEscuelaActual(EscuelaId);
            }
            return View(_indexViewModel);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}