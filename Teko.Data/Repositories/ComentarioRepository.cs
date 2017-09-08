using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Data.Infrastructure;
using Teko.Model;

namespace Teko.Data.Repositories
{
    public class ComentarioRepository : RepositoryBase<Comentarios>, IComentarioRepository
    {
        public ComentarioRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Comentarios> GetByContenido(int Id)
        {
            return DbContext.Comentarios.Where(x => x.ContenidoId == Id).Where(x => x.ParentId == null).ToList();
        }
    }
    public interface IComentarioRepository : IRepository<Comentarios>
    {
        List<Comentarios> GetByContenido(int Id);
    }
}
