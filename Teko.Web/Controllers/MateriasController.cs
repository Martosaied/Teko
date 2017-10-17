using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Model;
using Teko.Service;

namespace Teko.Web.Controllers
{
    public class MateriasController : Controller
    {
        private readonly IMateriaService materiaService;

        public MateriasController(IMateriaService materiaService)
        {
            this.materiaService = materiaService;
        }
        // GET: Materias
        public ActionResult Index()
        {
            Dictionary<string, List<Materias>> ListaMateriasPorLetra = new Dictionary<string, List<Materias>>();
            ListaMateriasPorLetra = materiaService.GetAllByLetter();
            ViewBag.Title = "Materias";
            return View("CosasPorLetra", ListaMateriasPorLetra);
        }
    }
}