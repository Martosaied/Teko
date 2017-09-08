using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class ValoracionRepository : RepositoryBase<Valoraciones>, IValoracionRepository
    {
        public ValoracionRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<int> GetByCont(int id)
        {
            return DbContext.Valoraciones.Where(x => x.Contenido.Id == id).Select(x => x.Valoracion).ToList();
        }
    }
    public interface IValoracionRepository : IRepository<Valoraciones>
    {
        List<int> GetByCont(int id);
    }
}
