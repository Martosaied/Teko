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
}