using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Model
{
    public class Reportes
    {
        public int Id { get; set; }

        [ForeignKey("ContenidoReportado")]
        public int IdContenido { get; set; }

        [ForeignKey("UserReportando")]
        public string IdUsuario { get; set; }

        public virtual Usuarios UserReportando { get; set; }
        public virtual Contenidos ContenidoReportado { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
    }
}
