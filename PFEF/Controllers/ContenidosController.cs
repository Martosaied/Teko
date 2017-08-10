using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PFEF.ViewModels;
using PFEF.Models;
using PFEF.Extensions;
using AutoMapper;
using EntityFramework.Extensions;
using PFEF.Models.DataAccess;

namespace PFEF.Controllers
{
    public class ContenidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private MuestraViewModel _ViewModel = new MuestraViewModel();
        // GET: Contenidos
        public ActionResult VerTodo(string Title)
        {
            _ViewModel = SwitchTitle(Title, _ViewModel);
            GuardarIds(_ViewModel.ListaAMostrar);
            _ViewModel.ChargeDrops();
            return View("MuestraCont",_ViewModel);
        }

        public JsonResult PopuladorEsc(int Lvl)
        {
            var Esc = db.Escuelas.Where(x=>x.NivEduEscuela.Id == Lvl).Select(c => new { Id = c.Id, Nombre = c.Nombre }).ToList();
            return Json(Esc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Descargar(int ID)
        {
            Contenidos selected = db.Contenidos.Find(ID);
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = "";
                ContenidosDA.UpdateIdes(false, selected);
                string contentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                return new FilePathResult("~/Content/Uploads/" + selected.Ruta, contentType)
                {
                    FileDownloadName = selected.Ruta,
                };
            }else
            {
                ViewBag.Error = "Necesita estar logueado para descargar documentos";
                return View("VerMas",selected);
            }
        }

        [HttpGet]
        public ActionResult Subir()
        {
            SubirViewModel SVM = new SubirViewModel();
            SVM.ChargeDrops();
            return View("SubirContenidos",SVM);
        }


        [HttpPost]
        public ActionResult Subir(SubirViewModel Cont, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Contenidos ContMapeado = HelpersExtensions.MappearContenidos(Cont);

                    string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                    archivo = archivo.Replace(" ", "");     
                    
                    ContMapeado.Ruta = archivo;
                    ContMapeado.UsuariosId = User.Identity.GetUserInfoId().Id;
                    ContMapeado.FechaSubida = DateTime.Now;
                    ContMapeado.IDes = 0;
                    ContMapeado.IPop = 0;
                                            
                    db.Contenidos.Add(ContMapeado);
                    db.SaveChanges();           

                    file.SaveAs(Server.MapPath("~/Content/Uploads/" + archivo));
                    return RedirectToAction("Index", "Home");
                }
                return View("SubirContenidos",Cont);
            }
            else
            {
                return View("SubirContenidos",Cont);
            }
        }

        [HttpGet]
        public ActionResult VerMas(int cont)
        {
            Contenidos SelectedCont = db.Contenidos.Find(cont);
            ContenidosDA.UpdateIdes(true, SelectedCont);
            if (Request.IsAuthenticated)
            {
                bool result = ContenidosDA.Recomendaciones.UpdateRecomendation(SelectedCont,User.Identity.GetUserInfoId());
            }
            var MappedCont = MapperContDetails(SelectedCont);
            MappedCont.Recomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(SelectedCont);
            ViewBag.URL = ObtenerURLArchivo(SelectedCont);
            ViewBag.Title = SelectedCont.Nombre;
            return View(MappedCont);
        }

        [HttpGet]
        public ActionResult Buscar(string Buscador)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = ContenidosDA._Searcher(Buscador);
            GuardarIds(_ViewModel.ListaAMostrar);
            _ViewModel.ChargeDrops();
            ViewBag.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
            return View("MuestraCont",_ViewModel);
        }

        [HttpPost]
        public ActionResult Buscar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            FilterParameters.ChargeDrops();
            var Lista = ObtenerIds();
            FilterParameters.ListaAMostrar = ContenidosDA._Filter(FilterParameters, Lista);
            GuardarIds(FilterParameters.ListaAMostrar);
            ViewBag.Title = FilterParameters.Title;
            return View("MuestraCont",FilterParameters);
        }

        public ActionResult Tag(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = ContenidosDA._SearcherByTag(Tag);
            GuardarIds(_ViewModel.ListaAMostrar);
            _ViewModel.ChargeDrops();
            ViewBag.Title = "Resultados: '" + Tag + "'";
            return View("MuestraCont", _ViewModel);
        }

        public ActionResult PasarPagina(int Pagina, string Title)
        {
            if (Pagina == -1)
            {
                Pagina = 0;
            }
            Session["Page"] = Pagina;
            _ViewModel.ListaAMostrar = ObtenerIds();
            _ViewModel.ChargeDrops();
             return View("MuestraCont",_ViewModel);
        }

        public PartialViewResult Valoration(int Id, int star,int? Val)
        {
            DetailsViewModel MappedCont;
            using(ApplicationDbContext dbval = new ApplicationDbContext())
            {
                if(Val == null) Val = 0;
                var Cont = dbval.Contenidos.Find(Id);
                Valoraciones MiVal = new Valoraciones()
                {
                    User = new Usuarios()
                    {
                        Id = User.Identity.GetUserInfoId().Id
                    },
                    Contenido = Cont,             
                    Valoracion = star                   
                };
                dbval.Valoraciones.Add(MiVal);
                dbval.SaveChanges();

                Cont.ValoracionPromedio = dbval.Valoraciones.Where(x => x.Contenido.Id == Id).Select(x => x.Valoracion).ToList().Average();
                Cont.ValoracionPromedio = Math.Round(Cont.ValoracionPromedio, 1);
                dbval.SaveChanges();
                MappedCont = MapperContDetails(Cont);
            }
            return PartialView("_Valoration",MappedCont);
        }
        #region Functions
        protected string ObtenerURLArchivo(Contenidos model)
        {
            string URL = model.Ruta.Substring(model.Ruta.Length - 4, 4);
            if (URL == ".pdf")
            {
                URL = "https://docs.google.com/viewer?url=https://tekoteko.azurewebsites.net/Content/Uploads/" + model.Ruta + "&embedded=true";
            }
            else
            {
                URL = "https://view.officeapps.live.com/op/embed.aspx?src=https://tekoteko.azurewebsites.net/Content/Uploads/" + model.Ruta;
            }
            return URL;
        }
        protected MuestraViewModel SwitchTitle(string Title, MuestraViewModel Parameters)
        {
            
            switch (Title)
            {
                case "Mas recientes":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.Id).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas populares":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas descargados":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas valorados":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.ValoracionPromedio).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Escuela":
                    int id = User.Identity.GetUserInfoId().InstitucionActual.Id;
                    _ViewModel.ListaAMostrar = db.Contenidos.Where(x => x.Escuelas.Id == id).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mis subidas":
                    _ViewModel.ListaAMostrar = User.Identity.GetUserInfoId().Contenidos.ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                default:
                    Parameters.ChargeDrops();
                    Parameters.ListaAMostrar = ContenidosDA._Searcher((string)Session["KeyWords"]);
                    Parameters.ListaAMostrar = ContenidosDA._Filter((MuestraViewModel)Session["Filters"], Parameters.ListaAMostrar);
                    ViewBag.Title = Title;
                    return Parameters;
            }
        }
        protected DetailsViewModel MapperContDetails(Contenidos cont)
        {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Contenidos, DetailsViewModel>();
                });
                IMapper mapper = config.CreateMapper();
                var ContMapeado = mapper.Map<Contenidos, DetailsViewModel>(cont);
                return ContMapeado;
        }
        protected void GuardarIds(Contenidos[] Ids)
        {
            var result = Ids.Select(x => x.Id).ToArray();
            Session["ListIds"] = result;
        }
        protected Contenidos[] ObtenerIds()
        {
            int[] ids = (int[])Session["ListIds"];
            Contenidos[] ListaLlena;
            using (ApplicationDbContext ListDb = new ApplicationDbContext())
            {
                ListaLlena = db.Contenidos.Where(x => ids.Contains(x.Id)).ToArray();
            }
            return ListaLlena;
        }

        #endregion
    }
}