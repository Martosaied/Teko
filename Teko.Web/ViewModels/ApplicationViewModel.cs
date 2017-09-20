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
}