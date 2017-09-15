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

        public Valoraciones GetValoracionByUserAndContenido(int id, string userId)
        {
            return DbContext.Valoraciones.Where(x => x.User_Id == userId && x.IdContenido == id).FirstOrDefault();
        }

        public List<int> GetValoracionesByContenido(int id)
        {
            return DbContext.Valoraciones.Where(x => x.Contenido.Id == id).Select(x => x.Valoracion).ToList();
        }
    }
    public interface IValoracionRepository : IRepository<Valoraciones>
    {
        List<int> GetValoracionesByContenido(int id);
        Valoraciones GetValoracionByUserAndContenido(int id, string userId);
    }
}
