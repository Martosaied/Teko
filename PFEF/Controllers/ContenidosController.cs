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
using Microsoft.AspNet.Identity;
using Ionic.Zip;
using System.IO;

namespace PFEF.Controllers
{
    public class ContenidosController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

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
            Esc.Add(new { Id = -1, Nombre = "----------" });
            Esc.Add(new { Id = 0, Nombre = "Otra escuela" });
            Esc = Esc.OrderBy(x => x.Id).ToList();
            return Json(Esc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Descargar(int ID)
        {
            var Files = db.Archivos.Where(x => x.IdContenido.Id == ID).ToArray();
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = "";
                ContenidosDA.UpdateIdes(false, ID);
                if (Files.Count() == 1)
                {
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    return new FilePathResult("~/Content/Uploads/" + Files[0].Ruta, contentType)
                    {
                        FileDownloadName = Files[0].Ruta,
                    };
                }
                else
                {
                    var outputStream = new MemoryStream();

                    using (var zip = new ZipFile())
                    {
                        foreach (var item in Files)
                        {
                            zip.AddEntry(item.Ruta, "content");
                        }
                        zip.Save(outputStream);
                    }

                    outputStream.Position = 0;
                    return File(outputStream, "application/zip", "archivos.zip");
                }
            }
            else
            {
                Contenidos selected = db.Contenidos.Find(ID);
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
        public ActionResult Subir(SubirViewModel Cont)
        {
            if (ModelState.IsValid)
            {
                if (Cont.Files != null)
                {
                    Contenidos ContMapeado = AutoMapperGeneric<SubirViewModel, Contenidos>.ConvertToDBEntity(Cont);                    

                    if (Cont.NuevaEsc != null)
                    {
                        Escuelas esc = new Escuelas
                        {
                            Nombre = Cont.NuevaEsc,
                            NivEduEscuela = db.NivelesEducativos.Where(x => x.Id == Cont.NivNuevaEsc).FirstOrDefault()
                        };
                        ContMapeado.Escuelas = esc;     
                    }

                    ContMapeado.UsuariosId = HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).Id;
                    ContMapeado.FechaSubida = DateTime.Now;
                    ContMapeado.IDes = 0;
                    ContMapeado.IPop = 0;
                                            
                    db.Contenidos.Add(ContMapeado);
                    db.SaveChanges();

                    foreach (var file in Cont.Files)
                    {
                        string fileName = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                        fileName = fileName.Replace(" ", "_");

                        Contenidos.Archivos MiArchivo = new Contenidos.Archivos
                        {
                            IdContenido = db.Contenidos.OrderByDescending(p => p.Id).FirstOrDefault(),
                            Ruta = fileName
                        };
                        db.Archivos.Add(MiArchivo);
                        file.SaveAs(Server.MapPath("~/Content/Uploads/" + fileName));
                    }
                    db.SaveChanges();
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
            ContenidosDA.UpdateIdes(true, cont);
            if (Request.IsAuthenticated)
            {
                bool result = ContenidosDA.Recomendaciones.UpdateRecomendation(SelectedCont,HelpersExtensions.ObtenerUser(User.Identity.GetUserId()));
            }
            var MappedCont = AutoMapperGeneric<Contenidos,DetailsViewModel>.ConvertToDBEntity(SelectedCont);
            MappedCont.Recomendaciones = ContenidosDA.Recomendaciones.ObtenerRec(SelectedCont);
            MappedCont.Rutas = ObtenerURLArchivo(SelectedCont);
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
        public PartialViewResult Buscar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            var Lista = ObtenerIds();
            FilterParameters.ListaAMostrar = ContenidosDA._Filter(FilterParameters, Lista);
            ViewBag.Title = FilterParameters.Title;
            return PartialView("_ContViewer",FilterParameters);
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
                        Id = HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).Id
                    },
                    Contenido = Cont,             
                    Valoracion = star                   
                };
                dbval.Valoraciones.Add(MiVal);
                dbval.SaveChanges();

                Cont.ValoracionPromedio = dbval.Valoraciones.Where(x => x.Contenido.Id == Id).Select(x => x.Valoracion).ToList().Average();
                Cont.ValoracionPromedio = Math.Round(Cont.ValoracionPromedio, 1);
                dbval.SaveChanges();
                MappedCont = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(Cont);
            }
            return PartialView("_Valoration",MappedCont);
        }

        public PartialViewResult Ordenar(int Ordenar)
        {
            Session["Page"] = 0;
            var List = ObtenerIds();
            switch (Ordenar)
            {
                case 1:
                    _ViewModel.ListaAMostrar = List.OrderByDescending(x => x.ValoracionPromedio).ToArray();
                    break;
                case 2:
                    _ViewModel.ListaAMostrar = List.OrderByDescending(x => x.IPop).ToArray();
                    break;
                case 3:
                    _ViewModel.ListaAMostrar = List.OrderByDescending(x => x.IDes).ToArray();
                    break;
                case 4:
                    _ViewModel.ListaAMostrar = List.OrderByDescending(x => x.FechaSubida).ToArray();
                    break;
                default:
                    _ViewModel.ListaAMostrar = List;
                    break;
            }
            GuardarIds(_ViewModel.ListaAMostrar);
            return PartialView("_ContViewer", _ViewModel);
        }
        #region Functions
        protected List<string> ObtenerURLArchivo(Contenidos model)
        {
            var Files = db.Archivos.Where(x => x.IdContenido.Id == model.Id).ToList();
            List<string> Lista = new List<string>();
            foreach (var file in Files)
            {
                string URL = file.Ruta.Substring(file.Ruta.Length - 4, 4);
                switch (URL)
                {
                    case ".pdf":
                        URL = "https://docs.google.com/viewer?url=https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta + "&embedded=true";
                        break;
                    case ".jpg":
                    case ".png":
                        URL = "https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta;
                        break;
                    default:
                        URL = "https://view.officeapps.live.com/op/embed.aspx?src=https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta;
                        break;
                }
                Lista.Add(URL);
            }
            
            return Lista;
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
                    int id = HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).InstitucionActual.Id;
                    _ViewModel.ListaAMostrar = db.Contenidos.Where(x => x.Escuelas.Id == id).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mis subidas":
                    _ViewModel.ListaAMostrar = HelpersExtensions.ObtenerUser(User.Identity.GetUserId()).Contenidos.ToArray();
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
        protected void GuardarIds(Contenidos[] Ids)
        {
            Session["ListIds"] = Ids;
        }
        protected Contenidos[] ObtenerIds()
        {
            Contenidos[] ListaLlena = (Contenidos[])Session["ListIds"];
            return ListaLlena;
        }

        #endregion
    }
}