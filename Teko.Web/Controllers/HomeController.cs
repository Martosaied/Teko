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

        public ActionResult MateriasPorLetra()
        {
            Dictionary<string, List<Materias>> ListaMateriasPorLetra = new Dictionary<string, List<Materias>>();
            ListaMateriasPorLetra = materiaService.GetAllByLetter();
            ViewBag.Title = "Materias";
            return View("CosasPorLetra", ListaMateriasPorLetra);
        }
        public ActionResult EscuelasPorLetra()
        {
            Dictionary<string, List<Escuelas>> ListaEscuelasPorLetra = new Dictionary<string, List<Escuelas>>();
            ListaEscuelasPorLetra = escuelaService.GetAllByLetter();
            ViewBag.Title = "Escuela";
            return View("CosasPorLetra", ListaEscuelasPorLetra);
        }
        public ActionResult Index()
        {
            Session["Page"] = 0;
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