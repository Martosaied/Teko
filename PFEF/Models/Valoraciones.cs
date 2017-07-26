using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFEF.Models
{
    public class Valoraciones
    {
        public int Id { get; set; }
        public virtual Usuarios User { get; set; }
        public virtual Contenidos Contenido { get; set; }
        public int Valoracion { get; set; }
    }
}