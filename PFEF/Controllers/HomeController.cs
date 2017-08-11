using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PFEF.Models;
using PFEF.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PFEF.Extensions;


namespace PFEF.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            MuestraViewModel MVM = new MuestraViewModel();
            Session["Page"] = 0;
             if (Request.IsAuthenticated)
            {
                if (HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).PerfilCompleto == true)
                {
                    var id = HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).InstitucionActualId;
                    ViewBag.ListaArticulosEsc = db.Contenidos.Where(x => x.EscuelasId == id).OrderByDescending(x => x.Id).Take(9).ToArray();
                }
                
            }
            ViewBag.ListaArticulos = db.Contenidos.OrderByDescending(x => x.Id).Take(9).ToArray();
            ViewBag.ListaArticulosPop = db.Contenidos.OrderByDescending(x => x.IPop).Take(9).ToArray();
            ViewBag.ListaArticulosDes = db.Contenidos.OrderByDescending(x => x.IDes).Take(9).ToArray();
            ViewBag.ListaArticulosVal = db.Contenidos.OrderByDescending(x => x.ValoracionPromedio).ToArray();
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
        [HttpPost]
        public ActionResult Escuela(string escuela)
        {
            TiposContenidos sc = new TiposContenidos()
            {
                Nombre = escuela
            };
            db.TiposContenidos.Add(sc);
            db.SaveChanges();
            return View("Escuelas");
        }
        [HttpGet]
        public ActionResult Escuela()
        {         
            return View("Escuelas");
        }
    }
}