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
using Teko.Web.ViewModels;

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
        public PartialViewResult Notificaciones()
        {
            List<Comentarios> ListaNotificaciones = comentarioService.GetNotificableComentariosByUser(User.Identity.GetUserId());
            NotificationViewModel viewModel = new NotificationViewModel(ListaNotificaciones);
            return PartialView("_Notification", viewModel);
        }
        // GET: Contenidos
        public ActionResult VerTodo(string Title)
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContsByTitle(Title);
            ViewBag.Title = Title;
            GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
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
        public ActionResult Descargar(int ContenidoId)
        {
            var Files = archivoService.GetArchivosByContenidoId(ContenidoId);
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = "";
                contenidoService.UpdateContenidoDescargas(ContenidoId);
                if (Files.Count() == 1)
                {
                    FilePathResult ArchivoDescarga = GetArchivoUnicoDescarga(Files);
                    return ArchivoDescarga;
                }
                else
                {
                    var ArchivoZipDescarga = GetZipDescarga(Files);
                    return File(ArchivoZipDescarga, "application/zip", "archivos.zip");
                }
            }
            else
            {
                Contenidos selected = contenidoService.GetContenidoById(ContenidoId);
                var Mapper = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(selected);
                ViewBag.Error = "Necesita estar logueado para descargar documentos";
                return View("VerMas",selected);
            }
        }

        #region Metodos de subir
        [HttpGet]
        public ActionResult Subir()
        {
            return View("Subir",SVM);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Subir(SubirViewModel Cont, string Contenido)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Cont.Files != null)
                    {
                        Contenidos ContMapeado = AutoMapperGeneric<SubirViewModel, Contenidos>.ConvertToDBEntity(Cont);
                        PrepareContenidoForUpload(Cont, ContMapeado);
                        contenidoService.CreateContenido(ContMapeado);
                        contenidoService.SaveContenido();

                        UploadFilesDB(Cont.Files);
                        return RedirectToAction("Index", "Home");
                    }
                }
                Cont.SetServices(escuelaService, tipoService, nivelService, materiaService);
                return View("Subir", Cont);
            }
            catch (Exception e)
            {
                return View("ErrorPage");
            }
        }
        private Contenidos PrepareContenidoForUpload(SubirViewModel Cont, Contenidos ContMapeado)
        {
            NuevaEscuela(Cont, ContMapeado);
            ContMapeado.UsuariosId = User.Identity.GetUserId();
            ContMapeado.FechaSubida = DateTime.Now;
            ContMapeado.IDes = 0;
            ContMapeado.IPop = 0;
            ContMapeado.Badget = contenidoService.SelectBadget(Cont.Files.ToList()[0].FileName);
            return ContMapeado;
        }
        private Contenidos NuevaEscuela(SubirViewModel Cont, Contenidos ContMapeado)
        {
            if (Cont.NuevaEsc != null)
            {
                ContMapeado.Escuelas = new Escuelas()
                {
                    NivEduEscuela_Id = Cont.NivNuevaEsc,
                    Nombre = Cont.NuevaEsc
                };
            }
            return ContMapeado;
        }
        private void UploadFilesDB(IEnumerable<HttpPostedFileBase> Files)
        {
            try
            {
                foreach (var file in Files)
                {
                    string fileName = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                    fileName = fileName.Replace(" ", "_");

                    Archivos MiArchivo = new Archivos(contenidoService.LastId(), fileName);
                    archivoService.AddArchivo(MiArchivo);
                    file.SaveAs(Server.MapPath("~/Content/Uploads/" + fileName));
                }
                archivoService.SaveArchivo();
            }catch(Exception e)
            {
                throw e;
            }
        }
        #endregion
        [HttpGet]
        public ActionResult VerMas(int cont)
        {
            Contenidos SelectedCont = contenidoService.GetContenidoById(cont);
            contenidoService.UpdateContenidoPopularidad(cont);
            UpdateVisitasIfAuthenticated(SelectedCont.Id);
            var MappedCont = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(SelectedCont);
            MappedCont.Recomendaciones = contenidoService.GetRecomendacionesByContenido(SelectedCont);
            var ListaArchivos = archivoService.GetArchivosByContenidoId(cont);
            MappedCont.ValoracionUsuarioActual = GetValoracionContenidoByUser(cont);
            MappedCont.Rutas = archivoService.GetURLToShowDocument(ListaArchivos);
            MappedCont.FormComentario = new FormComentario(comentarioService.GetByContenidos(SelectedCont.Id), cont);
            ViewBag.Title = SelectedCont.Nombre;
            return View(MappedCont);
        }
        private int GetValoracionContenidoByUser(int ContenidoId)
        {
            string UserId = User.Identity.GetUserId();
            Valoraciones ValoracionContenidoUsuario = valoracionService.GetValoracionByUserAndContenido(ContenidoId, UserId);
            return ValoracionContenidoUsuario.Valoracion;
        }
        private void UpdateVisitasIfAuthenticated(int IdContenido)
        {
            if (Request.IsAuthenticated)
            {
                string CurrentUserId = User.Identity.GetUserId();
                visitaService.UpdateVisitasByUser(IdContenido, CurrentUserId);
            }
        }
        public ActionResult AgregarComentario(FormComentario model)
        {
            Comentarios MappedComent = AutoMapperGeneric<FormComentario, Comentarios>.ConvertToDBEntity(model);
            MappedComent.UsuarioId = User.Identity.GetUserId();
            MappedComent.FechaPublicacion = DateTime.UtcNow;
            comentarioService.AddComentario(MappedComent);
            comentarioService.SaveComentario();
            return RedirectToAction("VerMas",new { cont =  model.ContenidoId });
        }
        [HttpGet]
        public ActionResult Buscar(string Buscador)
        {
            try
            {
                Session["Page"] = 0;
                _ViewModel.ListaAMostrar = contenidoService.GetContenidosByKeywords(Buscador);
                GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
                ViewBag.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
                return View("MuestraCont", _ViewModel);
            }
            catch (Exception e)
            {
                return View("ErrorPage");
            }
        }
        [HttpPost]
        public PartialViewResult Filtrar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            var Lista = ObtenerContenidosDesdeSession();
            var Contenido = AutoMapperGeneric<MuestraViewModel, Contenidos>.ConvertToDBEntity(FilterParameters);
            FilterParameters.ListaAMostrar = contenidoService.FiltrarContenidos(Contenido, Lista);
            ViewBag.Title = FilterParameters.Title;
            return PartialView("_ContenidoPagPrincipal",FilterParameters);
        }
        public ActionResult Tag(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosByTag(Tag);
            GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
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
            _ViewModel.ListaAMostrar = ObtenerContenidosDesdeSession();
             return View("MuestraCont",_ViewModel);
        }
        public PartialViewResult Valoration(int ContenidoId, int Val,DetailsViewModel star)
        {
            if (Val == null) Val = 0;
            var Cont = contenidoService.GetContenidoById(ContenidoId);
            string UserId = User.Identity.GetUserId();
            Valoraciones NuevaValoracion = new Valoraciones(UserId, ContenidoId, star.ValoracionUsuarioActual);
            DeletePreviousValoraciones(ContenidoId, UserId);
            valoracionService.AddValoracion(NuevaValoracion);
            valoracionService.SaveValoracion();

            Cont.ValoracionPromedio = valoracionService.GetPromedioValoracionesByContenidos(ContenidoId);
            Cont.ValoracionPromedio = Math.Round(Cont.ValoracionPromedio, 1);
            valoracionService.SaveValoracion();
            DetailsViewModel MappedCont = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(Cont);
            return PartialView("_Valoration", MappedCont);
        }
        private void DeletePreviousValoraciones(int Id, string UserId)
        {
            Valoraciones ValoracionVieja = valoracionService.GetValoracionByUserAndContenido(Id, UserId);
            if (ValoracionVieja != null)
            {
                valoracionService.DeleteValoracion(ValoracionVieja);

            }
        }
        public PartialViewResult Ordenar(int Ordenar)
        {
            Session["Page"] = 0;
            var List = ObtenerContenidosDesdeSession();
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
            GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
            return PartialView("_ContenidoPagPrincipal", _ViewModel);
        }

        #region Functions
        private MemoryStream GetZipDescarga(List<Archivos> Files)
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
            return outputStream;
        }
        private FilePathResult GetArchivoUnicoDescarga(List<Archivos> Files)
        {
            string contentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
             return new FilePathResult("~/Content/Uploads/" + Files[0].Ruta, contentType)
             {
                 FileDownloadName = Files[0].Ruta,
             };
        }
        protected MuestraViewModel SwitchTitle(string Title, MuestraViewModel Parameters)
        {
            
            switch (Title)
            {
                case "Mas recientes":
                    _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderRecent();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas populares":
                    _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderPopular();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas descargados":
                    _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderDescargas();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas valorados":
                    _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderValoracion();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Escuela":
                    int id = usuarioService.GetUserById(User.Identity.GetUserId()).InstitucionActual.Id;
                    _ViewModel.ListaAMostrar = contenidoService.GetContenidosByEscuela(id);
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mis subidas":
                    _ViewModel.ListaAMostrar = usuarioService.GetUserById(User.Identity.GetUserId()).Contenidos.ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                default:
                    Parameters.ListaAMostrar = contenidoService.GetContenidosByKeywords((string)Session["KeyWords"]);
                    var Filtro = AutoMapperGeneric<MuestraViewModel, Contenidos>.ConvertToDBEntity((MuestraViewModel)Session["Filters"]);
                    Parameters.ListaAMostrar = contenidoService.FiltrarContenidos(Filtro, Parameters.ListaAMostrar);
                    ViewBag.Title = Title;
                    return Parameters;
            }
        }
        protected void GuardarContenidosEnSession(Contenidos[] Ids)
        {
            Session["ListIds"] = Ids;
        }
        protected Contenidos[] ObtenerContenidosDesdeSession()
        {
            Contenidos[] ListaLlena = (Contenidos[])Session["ListIds"];
            return ListaLlena;
        }
        #endregion
    }
}