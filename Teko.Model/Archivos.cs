using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Model
{
    public class Archivos
    {
        public Archivos(int IdContenido, string RutaGuardado)
        {
            this.IdContenido = IdContenido;
            this.Ruta = RutaGuardado;
        }
        public Archivos()
        {

        }
        public int Id { get; set; }
        [ForeignKey("Contenido")]public int IdContenido { get; set; }
        public virtual Contenidos Contenido { get; set; }
        public string Ruta { get; set; }
    }
}
