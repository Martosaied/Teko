using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Service;

namespace Teko.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContenidoService contenidoService;
        private readonly IArchivoService archivoService;
        private readonly IEscuelaService escuelaService;
        private readonly IVisitaService visitaService;
        private readonly IUsuarioService usuarioService;

        public HomeController(IContenidoService contenidoService, IArchivoService archivoService, IEscuelaService escuelaService, IVisitaService visitaService, IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
            this.contenidoService = contenidoService;
            this.archivoService = archivoService;
            this.escuelaService = escuelaService;
            this.visitaService = visitaService;
        }

        public HomeController()
        {

        }
        public ActionResult Index()
        {
            //return RedirectToAction("Login", "Account");
            //var MiUser = HelpersExtensions.ObtenerUser(User.Identity.GetUserId());
            Session["Page"] = 0;
             if (Request.IsAuthenticated)
            {
                
            }
            ViewBag.ListaArticulos = contenidoService.GetContenidosOrderRecent().Take(9).ToArray();
            ViewBag.ListaArticulosPop = contenidoService.GetContenidosOrderPopular().Take(9).ToArray();
            ViewBag.ListaArticulosDes = contenidoService.GetContenidosOrderDescargas().Take(9).ToArray();
            ViewBag.ListaArticulosVal = contenidoService.GetContenidosOrderValoracion().Take(9).ToArray();
            return View();

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