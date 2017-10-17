using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Model;
using Teko.Service;

namespace Teko.Web.Controllers
{
    public class EscuelasController : Controller
    {
        private readonly IEscuelaService escuelaService;

        public EscuelasController(IEscuelaService escuelaService)
        {
            this.escuelaService = escuelaService;
        }
        // GET: Escuelas
        public ActionResult Index()
        {
            Dictionary<string, List<Escuelas>> ListaEscuelasPorLetra = new Dictionary<string, List<Escuelas>>();
            ListaEscuelasPorLetra = escuelaService.GetAllByLetter();
            ViewBag.Title = "Escuelas";
            return View("CosasPorLetra", ListaEscuelasPorLetra);
        }
    }
}