using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class ValoracionRepository : RepositoryBase<Valoraciones>, IValoracionRepository
    {
        public ValoracionRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface IValoracionRepository : IRepository<Valoraciones>
    {

    }
}
