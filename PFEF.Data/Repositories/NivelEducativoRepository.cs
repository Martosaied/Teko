using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class NivelEducativoRepository : RepositoryBase<NivelesEducativos>, INivelEducativoRepository
    {
        public NivelEducativoRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
    }
    public interface INivelEducativoRepository : IRepository<NivelesEducativos>
    {

    }
}
