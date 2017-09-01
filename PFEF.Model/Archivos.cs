using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Model
{
    public class Archivos
    {
        public int Id { get; set; }
        public virtual Contenidos IdContenido { get; set; }
        public string Ruta { get; set; }
    }
}
