using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Model
{
    public class Comentarios
    {
        public int Id { get; set; }
        public Nullable<int>ParentId{ get; set; }
        public string Texto { get; set; }
        public int ContenidoId { get; set; }
        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]public virtual Usuarios Usuario { get; set; }
        [ForeignKey("ContenidoId")]public virtual Contenidos Contenido { get; set; }
        [ForeignKey("ParentId")]public virtual Comentarios ComentarioPadre { get; set; }

        public virtual ICollection<Comentarios> ComentariosHijos{ get; set; }
    }
}
