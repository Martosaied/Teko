using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class ArchivosRepository : RepositoryBase<Archivos>, IArchivosRepository
    {
        public ArchivosRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Archivos> GetByContenido(int id)
        {
            return DbContext.Archivos.Where(x => x.IdContenido.Id == id).ToList();
        }
    }
    public interface IArchivosRepository : IRepository<Archivos>
    {
        List<Archivos> GetByContenido(int id);
    }
}
