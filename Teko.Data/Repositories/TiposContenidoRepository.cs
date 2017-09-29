using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class TiposContenidoRepository : RepositoryBase<TiposContenidos>, ITiposContenidoRepository
    {
        public TiposContenidoRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface ITiposContenidoRepository : IRepository<TiposContenidos>
    {

    }
}
