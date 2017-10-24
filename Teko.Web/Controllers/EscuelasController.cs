using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Controllers;
using Teko.Model;
using Teko.Service;
using Teko.ViewModels;

namespace Teko.Web.Controllers
{
    public class EscuelasController : Controller
    {
        private readonly IMateriaService materiaService;
        private readonly IContenidoService contenidoService;
        private readonly ITipoService tipoService;
        private readonly INivelEducativoService nivelService;
        private readonly IEscuelaService escuelaService;
        MuestraViewModel _ViewModel;


        public EscuelasController(IMateriaService materiaService, IContenidoService contenidoService, ITipoService tipoService, INivelEducativoService nivelService, IEscuelaService escuelaService)
        {
            _ViewModel = new MuestraViewModel(escuelaService, tipoService, nivelService, materiaService);
            this.contenidoService = contenidoService;
            this.escuelaService = escuelaService;
        }
        // GET: Escuelas
        public ActionResult Index()
        {
            Dictionary<string, List<Escuelas>> ListaEscuelasPorLetra = new Dictionary<string, List<Escuelas>>();
            ListaEscuelasPorLetra = escuelaService.GetAllByLetter();
            ViewBag.Title = "Escuelas";
            dynamic model = new System.Dynamic.ExpandoObject();
            model.Lista = ListaEscuelasPorLetra;
            model.EsEscuela = true;
            return View("CosasPorLetra", model);
        }
        public ActionResult Contenidos(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosByTag(Tag);
            GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
            _ViewModel.Preseleccionar("Escuelas", Tag);
            _ViewModel.Title = "Resultados: '" + Tag + "'";
            return View("MuestraCont", _ViewModel);
        }
        public void GuardarContenidosEnSession(Contenidos[] Ids)
        {
            Session["ListIds"] = Ids;
        }
        public Contenidos[] ObtenerContenidosDesdeSession()
        {
            Contenidos[] ListaLlena = (Contenidos[])Session["ListIds"];
            return ListaLlena;
        }

    }
}