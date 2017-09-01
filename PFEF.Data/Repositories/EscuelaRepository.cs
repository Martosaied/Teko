using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class EscuelaRepository : RepositoryBase<Escuelas>, IEscuelaRepository
    {
        public EscuelaRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Escuelas> GetEscuelasByNivel(int id)
        {
            return DbContext.Escuelas.Where(x => x.NivEduEscuela.Id == id).ToList();
        }
    }
    public interface IEscuelaRepository : IRepository<Escuelas>
    {
        List<Escuelas> GetEscuelasByNivel(int id);
    }
}
