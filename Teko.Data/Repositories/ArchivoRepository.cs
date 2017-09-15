using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class ArchivosRepository : RepositoryBase<Archivos>, IArchivosRepository
    {
        public ArchivosRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Archivos> GetArchivosByContenido(int id)
        {
            return DbContext.Archivos.Where(x => x.IdContenido == id).ToList();
        }
    }
    public interface IArchivosRepository : IRepository<Archivos>
    {
        List<Archivos> GetArchivosByContenido(int id);
    }
}
