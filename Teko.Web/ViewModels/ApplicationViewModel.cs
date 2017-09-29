using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teko.Model;
using Teko.Service;

namespace Teko.Web.ViewModels
{
    public class ApplicationViewModel
    {

    }

    public class NotificationViewModel
    {
        public NotificationViewModel(List<Comentarios> ListaComentariosAAsignar)
        {
            ListaComentariosDeNotificaciones = ListaComentariosAAsignar;
        }
        public List<Comentarios> ListaComentariosDeNotificaciones { get; set; }
    }

    public class CosasViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual ICollection<Contenidos> Contenidos { get; set; }
    }

    public class IndexViewModel
    {
        IContenidoService contenidoService;
        IEscuelaService escuelaService;
        public IndexViewModel(IContenidoService contenidoService, IEscuelaService escuelaService)
        {
            this.contenidoService = contenidoService;
            this.escuelaService = escuelaService;
            ListaArticulosDes = contenidoService.GetContenidosOrderDescargas().Take(9).ToArray(); 
            ListaArticulosPop = contenidoService.GetContenidosOrderPopular().Take(9).ToArray(); 
            ListaArticulosVal = contenidoService.GetContenidosOrderValoracion().Take(9).ToArray(); 
            ListaArticulosRec = contenidoService.GetContenidosOrderRecent().Take(9).ToArray();
        }
        public void setListaArticulosEsc(int IdEscuelaUser)
        {
            ListaArticulosEsc = contenidoService.GetContenidosByEscuela(IdEscuelaUser).Take(9).ToArray();
        }
        public void setEscuelaActual(int IdEscuelaUser)
        {
            EscuelaAlumno = escuelaService.GetEscuelaById(IdEscuelaUser);
            EscuelaAlumno = (EscuelaAlumno == null) ? new Escuelas() : EscuelaAlumno;
        }
        public Contenidos[] ListaArticulosRec { get; set; }
        public Contenidos[] ListaArticulosPop { get; set; }
        public Contenidos[]ListaArticulosDes { get; set; }
        public Contenidos[] ListaArticulosVal { get; set; }
        public Contenidos[] ListaArticulosEsc { get; set; }
        public Escuelas EscuelaAlumno { get; set; }

    }
}