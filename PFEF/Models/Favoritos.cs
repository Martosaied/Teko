using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFEF.Models
{
    public class Favoritos
    {
        public int Id { get; set; }
        public virtual Usuarios IdUsuario { get; set; }
        public virtual Contenidos IdContenido { get; set; }
    }
}