﻿using System;
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
using Teko.Web.Extensions;

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
        private readonly IMailService mailService;
        private readonly IReportesService reportService;
        private MuestraViewModel _ViewModel;
        SubirViewModel SVM;
        private readonly ITipoService tipoService;
        public ContenidosController(IReportesService reportService,IMailService mailService,   IComentarioService comentarioService,IMateriaService materiaService, INivelEducativoService nivelService, ITipoService tipoService, IValoracionService valoracionService, IContenidoService contenidoService, IArchivoService archivoService, IEscuelaService escuelaService, IVisitaService visitaService, IUsuarioService usuarioService)
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
            this.mailService = mailService;
            this.comentarioService = comentarioService;
            this.reportService = reportService;
            _ViewModel = new MuestraViewModel(escuelaService, tipoService, nivelService, materiaService);
            SVM = new SubirViewModel(escuelaService, tipoService, nivelService, materiaService);
        }

        public void Reportar(ReportViewModel viewModel)
        {
            var UserId = User.Identity.GetUserId();
            var IUser = usuarioService.GetUserById(UserId);
            var ReportedContenido = contenidoService.GetContenidoById(viewModel.IdContenido);

            viewModel.reportedContenido = ReportedContenido;
            viewModel.reportedUser = IUser;

            bool work = mailService.SendReportEmail(viewModel);

            viewModel.IdUsuario = viewModel.reportedUser.Id;
            GuardarReport(viewModel);
            VerificarContenido(ReportedContenido);
        }

        private void VerificarContenido(Contenidos ReportedContenido)
        {
            int CantidadReportes = reportService.GetNumberReportsPerContent(ReportedContenido.Id);
            if(CantidadReportes > 5)
            {
                contenidoService.BajaLogica(ReportedContenido.Id);
                contenidoService.SaveContenido();
            }
        }

        private void GuardarReport(ReportViewModel viewModel)
        {
            var MappedReport = AutoMapperGeneric<ReportViewModel, Reportes>.ConvertToDBEntity(viewModel);
            reportService.AddReport(MappedReport);
            reportService.SaveReport();
        }
        public PartialViewResult Notificaciones()
        {
            List<Comentarios> ListaNotificaciones = comentarioService.GetNotificableComentariosByUser(User.Identity.GetUserId());
            NotificationViewModel viewModel = new NotificationViewModel(ListaNotificaciones);
            return PartialView("_Notification", viewModel);
        }
        public JsonResult PopuladorEsc(int Lvl)
        {
            var Esc = escuelaService.GetEscuelasByNivel(Lvl).Select(c => new { Id = c.Id, Nombre = c.Nombre }).ToList();
            Esc.Add(new { Id = -1, Nombre = "----------" });
            Esc.Add(new { Id = 0, Nombre = "Otra escuela" });
            Esc = Esc.OrderBy(x => x.Id).ToList();
            return Json(Esc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PopuladorEscFiltro(int Lvl)
        {
            var Esc = escuelaService.GetEscuelasByNivel(Lvl).Select(c => new { Id = c.Id, Nombre = c.Nombre }).ToList();
            Esc.Add(new { Id = -1, Nombre = "----------" });
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
                return View("Detalles",selected);
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
            Cont.SetServices(escuelaService, tipoService, nivelService, materiaService);
            if (ModelState.IsValid)
            {
                if (Cont.Files != null)
                {
                    Contenidos ContMapeado = ContenidoMapper.ConvertVMtoContenido(Cont);
                    PrepareContenidoForUpload(Cont, ContMapeado);
                    contenidoService.CreateContenido(ContMapeado);
                    contenidoService.SaveContenido();

                    UploadFilesDB(Cont.Files);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Cont.SetServices(escuelaService, tipoService, nivelService, materiaService);
                    return View("Subir", Cont);
                }
            }
            else
            {
                Cont.SetServices(escuelaService, tipoService, nivelService, materiaService);
                return View("Subir", Cont);
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
        public ActionResult Detalles(int id)
        {
            Contenidos SelectedCont = contenidoService.GetContenidoById(id);
            contenidoService.UpdateContenidoPopularidad(id);
            UpdateVisitasIfAuthenticated(SelectedCont.Id);
            var MappedCont = AutoMapperGeneric<Contenidos, DetailsViewModel>.ConvertToDBEntity(SelectedCont);
            MappedCont.reportService = reportService;
            MappedCont.Recomendaciones = contenidoService.GetRecomendacionesByContenido(SelectedCont);
            var ListaArchivos = archivoService.GetArchivosByContenidoId(id);
            if(Request.IsAuthenticated)
                MappedCont.ValoracionUsuarioActual = GetValoracionContenidoByUser(id);
            MappedCont.Rutas = archivoService.GetURLToShowDocument(ListaArchivos);
            MappedCont.FormComentario = new FormComentario(comentarioService.GetByContenidos(SelectedCont.Id), id);
            ViewBag.Title = SelectedCont.Nombre;
            return View(MappedCont);
        }
        private int GetValoracionContenidoByUser(int ContenidoId)
        {
            string UserId = User.Identity.GetUserId();
            Valoraciones ValoracionContenidoUsuario = valoracionService.GetValoracionByUserAndContenido(ContenidoId, UserId);
            return (ValoracionContenidoUsuario == null) ? 0 : ValoracionContenidoUsuario.Valoracion;
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
            Contenidos ContenidoARedirigir = contenidoService.GetContenidoById(model.ContenidoId);
            return RedirectToAction("Detalles", new { id = SlugGenerator.GenerateContenidoSlug(ContenidoARedirigir.Id, ContenidoARedirigir.Nombre) });
        }
        public ActionResult Buscar(string Buscador)
        {
            try
            {
                Session["Page"] = 0;
                _ViewModel.ListaAMostrar = contenidoService.GetContenidosByKeywords(Buscador);
                GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
                _ViewModel.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
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
            var Contenido = ContenidoMapper.ConvertVMtoContenido(FilterParameters);
            FilterParameters.ListaAMostrar = contenidoService.FiltrarContenidos(Contenido, Lista);
            _ViewModel.Title = FilterParameters.Title;
            return PartialView("_ContenidoPagPrincipal",FilterParameters);
        }
        public ActionResult Tag(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosByTag(Tag);
            GuardarContenidosEnSession(_ViewModel.ListaAMostrar);
            _ViewModel.Title = "Resultados: '" + Tag + "'";
            return View("MuestraCont", _ViewModel);
        }
        public PartialViewResult PasarPagina(int Pagina)
        {
            if (Pagina == -1)
            {
                Pagina = 0;
            }
            Session["Page"] = Pagina;
            _ViewModel.ListaAMostrar = ObtenerContenidosDesdeSession();
             return PartialView("_ContenidoPagPrincipal",_ViewModel);
        }
        public PartialViewResult Valoration(int ContenidoId,DetailsViewModel star)
        {
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
        public ViewResult GetContenidos( MuestraViewModel viewModel)
        {
            GuardarContenidosEnSession(viewModel.ListaAMostrar);
            return View("MuestraCont", viewModel);
        }
        public ActionResult Recientes()
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderRecent();
            _ViewModel.Title = "Mas recientes";
            return GetContenidos(_ViewModel);
        }
        public ActionResult Populares()
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderPopular();
            _ViewModel.Title = "Mas populares";
            return GetContenidos(_ViewModel);
        }
        public ActionResult Descargados()
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderDescargas();
            _ViewModel.Title = "Mas descargados";
            return GetContenidos(_ViewModel);
        }
        public ActionResult Valorados()
        {
            _ViewModel.ListaAMostrar = contenidoService.GetContenidosOrderValoracion();
            _ViewModel.Title = "Mas valorados";
            return GetContenidos(_ViewModel);
        }
        public ActionResult Subidas()
        {
            _ViewModel.ListaAMostrar = usuarioService.GetUserById(User.Identity.GetUserId()).Contenidos.ToArray();
            _ViewModel.Title = "Mis subidas";
            return GetContenidos(_ViewModel);
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
        public void GuardarContenidosEnSession(Contenidos[] Ids)
        {
            Session["ListIds"] = Ids;
        }
        public Contenidos[] ObtenerContenidosDesdeSession()
        {
            Contenidos[] ListaLlena = (Contenidos[])Session["ListIds"];
            return ListaLlena;
        }
        #endregion
    }
}