using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.ViewModels;
using Teko.Models;
using Teko.Extensions;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Ionic.Zip;
using System.IO;
using Teko.Service;
using Teko.Model;

namespace Teko.Controllers
{
    public class ContenidosController : Controller
    {
        private readonly IContenidoService contenidoService;
        private readonly IArchivoService archivoService;
        private readonly IEscuelaService escuelaService;
        private readonly IVisitaService visitaService;
        private readonly IUsuarioService usuarioService;
        private readonly IValoracionService valoracionService;
        private readonly IMateriaService materiaService;
        private readonly INivelEducativoService nivelService;
        private readonly IComentarioService comentarioService;
        private MuestraViewModel _ViewModel;
        SubirViewModel SVM;
        private readonly ITipoService tipoService;
        public ContenidosController(IComentarioService comentarioService,IMateriaService materiaService, INivelEducativoService nivelService, ITipoService tipoService, IValoracionService valoracionService, IContenidoService contenidoService, IArchivoService archivoService, IEscuelaService escuelaService, IVisitaService visitaService, IUsuarioService usuarioService)
        {
            this.usuarioService = usuarioService;
            this.contenidoService = contenidoService;
            this.archivoService = archivoService;
            this.escuelaService = escuelaService;
            this.visitaService = visitaService;
            this.valoracionService = valoracionService;
            this.tipoService = tipoService;
            this.nivelService = nivelService;
            this.materiaService = materiaService;
            this.comentarioService = comentarioService;
            _ViewModel = new MuestraViewModel(escuelaService, tipoService, nivelService, materiaService);
            SVM = new SubirViewModel(escuelaService, tipoService, nivelService, materiaService);
        }


        // GET: Contenidos
        public ActionResult VerTodo(string Title)
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContsByTitle(Title);
            ViewBag.Title = Title;
            GuardarIds(_ViewModel.ListaAMostrar);
            //_ViewModel = ChargeDrops(_ViewModel);
            return View("MuestraCont",_ViewModel);
        }

        public JsonResult PopuladorEsc(int Lvl)
        {
            var Esc = escuelaService.GetEscuelasByNivel(Lvl).Select(c => new { Id = c.Id, Nombre = c.Nombre }).ToList();
            Esc.Add(new { Id = -1, Nombre = "----------" });
            Esc.Add(new { Id = 0, Nombre = "Otra escuela" });
            Esc = Esc.OrderBy(x => x.Id).ToList();
            return Json(Esc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Descargar(int ID)
        {
            var Files = archivoService.GetByContenido(ID);
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = "";
                contenidoService.UpdateDes(ID);
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
                Contenidos selected = contenidoService.GetContById(ID);
                var Mapper = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(selected);
                ViewBag.Error = "Necesita estar logueado para descargar documentos";
                return View("VerMas",selected);
            }
        }

        [HttpGet]
        public ActionResult Subir()
        {
            //SVM = ChargeDrops(SVM);
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
                        ContMapeado.Escuelas.Nombre = Cont.NuevaEsc;
                        ContMapeado.Escuelas.NivEduEscuela.Id = Cont.NivNuevaEsc;   
                    }

                    ContMapeado.UsuariosId = User.Identity.GetUserId();
                    ContMapeado.FechaSubida = DateTime.Now;
                    ContMapeado.IDes = 0;
                    ContMapeado.IPop = 0;
                    ContMapeado.Badget = contenidoService.BudgetSelect(Cont.Files.ToList()[0].FileName);
                                            
                    contenidoService.CreateContenido(ContMapeado);
                    contenidoService.SaveContenido();

                    foreach (var file in Cont.Files)
                    {
                        string fileName = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                        fileName = fileName.Replace(" ", "_");

                        Archivos MiArchivo = new Archivos
                        {
                            IdContenido = contenidoService.LastId(),
                            Ruta = fileName
                        };
                        archivoService.Crear(MiArchivo);
                        file.SaveAs(Server.MapPath("~/Content/Uploads/" + fileName));
                    }
                    archivoService.SaveArchivo();
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
            Contenidos SelectedCont = contenidoService.GetContById(cont);
            contenidoService.UpdatePop(cont);
            if (Request.IsAuthenticated)
            {
               visitaService.UpdateRecomendation(SelectedCont,usuarioService.GetById(User.Identity.GetUserId()));
            }
            var MappedCont = AutoMapperGeneric<Contenidos,DetailsViewModel>.ConvertToDBEntity(SelectedCont);
            MappedCont.Recomendaciones = contenidoService.ObtenerRecByCont(SelectedCont);
            MappedCont.Rutas = archivoService.ObtenerURLArchivos(SelectedCont.Id);
            MappedCont.FormComentario = new FormComentario(comentarioService.GetByContenidos(SelectedCont.Id), cont);
            ViewBag.Title = SelectedCont.Nombre;
            return View(MappedCont);
        }
        public ActionResult AgregarComentario(FormComentario model)
        {
            var MappedComent = AutoMapperGeneric<FormComentario, Comentarios>.ConvertToDBEntity(model);
            MappedComent.UsuarioId = User.Identity.GetUserId();
            comentarioService.AgregarComentario(MappedComent);
            comentarioService.SaveComentario();
            return RedirectToAction("VerMas",new { cont =  model.ContenidoId });
        }
        [HttpGet]
        public ActionResult Buscar(string Buscador)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = contenidoService.Search(Buscador);
            GuardarIds(_ViewModel.ListaAMostrar);
            ViewBag.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
            return View("MuestraCont",_ViewModel);
        }

        [HttpPost]
        public PartialViewResult Buscar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            var Lista = ObtenerIds();
            var Contenido = AutoMapperGeneric<MuestraViewModel, Contenidos>.ConvertToDBEntity(FilterParameters);
            FilterParameters.ListaAMostrar = contenidoService._Filter(Contenido, Lista);
            ViewBag.Title = FilterParameters.Title;
            return PartialView("_ContViewer",FilterParameters);
        }

        public ActionResult Tag(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = contenidoService.GetContByTag(Tag);
            GuardarIds(_ViewModel.ListaAMostrar);
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
             return View("MuestraCont",_ViewModel);
        }

        public PartialViewResult Valoration(int Id, int star,int? Val)
        {
            DetailsViewModel MappedCont;
                if(Val == null) Val = 0;
                var Cont = contenidoService.GetContById(Id);
                Valoraciones MiVal = new Valoraciones()
                {

                    User_Id = User.Identity.GetUserId(),
                    Contenido = Cont,             
                    Valoracion = star                   
                };
                valoracionService.Agregar(MiVal);
                valoracionService.SaveVal();

            Cont.ValoracionPromedio = valoracionService.GetPromVal(Id);
                Cont.ValoracionPromedio = Math.Round(Cont.ValoracionPromedio, 1);
                valoracionService.SaveVal();
                MappedCont = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(Cont);
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
        protected MuestraViewModel SwitchTitle(string Title, MuestraViewModel Parameters)
        {
            
            switch (Title)
            {
                case "Mas recientes":
                    _ViewModel.ListaAMostrar = contenidoService.GetByRec();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas populares":
                    _ViewModel.ListaAMostrar = contenidoService.GetByPop();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas descargados":
                    _ViewModel.ListaAMostrar = contenidoService.GetByDes();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas valorados":
                    _ViewModel.ListaAMostrar = contenidoService.GetByVal();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Escuela":
                    int id = usuarioService.GetById(User.Identity.GetUserId()).InstitucionActual.Id;
                    _ViewModel.ListaAMostrar = contenidoService.GetByEsc(id);
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mis subidas":
                    _ViewModel.ListaAMostrar = usuarioService.GetById(User.Identity.GetUserId()).Contenidos.ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                default:
                    Parameters.ListaAMostrar = contenidoService.Search((string)Session["KeyWords"]);
                    var Filtro = AutoMapperGeneric<MuestraViewModel, Contenidos>.ConvertToDBEntity((MuestraViewModel)Session["Filters"]);
                    Parameters.ListaAMostrar = contenidoService._Filter(Filtro, Parameters.ListaAMostrar);
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