using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Teko.Model
{
    public class Valoraciones
    {
        public Valoraciones(string UserId, int IdContenido, int Valoracion)
        {
            this.User_Id = UserId;
            this.IdContenido = IdContenido;
            this.Valoracion = Valoracion; 
        }

        public Valoraciones()
        {
        }

        public int Id { get; set; }
        public virtual Usuarios User { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        [ForeignKey("Contenido")]
        public int IdContenido { get; set; }
        public virtual Contenidos Contenido { get; set; }

        public int Valoracion { get; set; }
    }
}