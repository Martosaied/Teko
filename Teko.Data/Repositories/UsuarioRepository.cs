using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuarios>, IUsuarioRepository
    {
        public UsuarioRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Usuarios GetById(string id)
        {
            return DbContext.Users.Where(x => x.Id == id).FirstOrDefault();
        }
    }
    public interface IUsuarioRepository : IRepository<Usuarios>
    {
        Usuarios GetById(string id);
    }
}
