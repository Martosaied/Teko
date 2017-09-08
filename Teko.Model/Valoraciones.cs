using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teko.Model
{
    public class Valoraciones
    {
        public int Id { get; set; }
        public virtual Usuarios User { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual Contenidos Contenido { get; set; }
        public int Valoracion { get; set; }
    }
}