using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teko.Extensions;
using Teko.Model;
using Teko.Service;
using Teko.Web.ViewModels;

namespace Teko.Web.Controllers
{
    public class ReportsController : Controller
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
        private readonly ITipoService tipoService;
        public ReportsController(IReportesService reportService, IMailService mailService, IComentarioService comentarioService, IMateriaService materiaService, INivelEducativoService nivelService, ITipoService tipoService, IValoracionService valoracionService, IContenidoService contenidoService, IArchivoService archivoService, IEscuelaService escuelaService, IVisitaService visitaService, IUsuarioService usuarioService)
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
        }
        
    }
}