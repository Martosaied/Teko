using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuarios>, IUsuarioRepository
    {
        public UsuarioRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IUsuarioRepository : IRepository<Usuarios>
    {

    }
}
