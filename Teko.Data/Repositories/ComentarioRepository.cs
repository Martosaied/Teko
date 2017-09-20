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

        public List<Comentarios> GetComentariosByContenido(int Id)
        {
            return DbContext.Comentarios.Where(x => x.ContenidoId == Id).Where(x => x.ParentId == null).ToList();
        }

        public List<Comentarios> GetNotificableComentariosByUser(string UserId)
        {
            return DbContext.Comentarios.Where(x => x.Contenido.UsuariosId == UserId && x.IsNotificable == true).OrderByDescending(x => x.Id).ToList();
        }
    }
    public interface IComentarioRepository : IRepository<Comentarios>
    {
        List<Comentarios> GetComentariosByContenido(int Id);
        List<Comentarios> GetNotificableComentariosByUser(string UserId);
    }
}
