using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class VisitaRepository : RepositoryBase<Visitas>,IVisitaRepository 
    {
        public VisitaRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Visitas> GetVisitasByUserId(string Id)
        {
            var Dia20Atras = DateTime.UtcNow.AddDays(-20);
            var UltimasVisitas = DbContext.Visitas.Where(x => x.User.Id == Id && x.LastUpdate > Dia20Atras).OrderByDescending(x => x.Contador).Take(5).ToList();

            return UltimasVisitas;
        }
        public void UpdateVisitasByUser(int ContenidoId, string UserId)
        {
            var result = DbContext.Visitas.Where(x => x.User.Id == UserId && x.Contenido.Id == ContenidoId).FirstOrDefault();

            if (result != null)
            {
                DbContext.Visitas.Attach(result);
                if (result.LastUpdate < DateTime.UtcNow.AddDays(-20))
                {
                    result.Contador++;
                }
                else
                {
                    result.Contador = 1;
                }
                result.LastUpdate = DateTime.UtcNow;
                DbContext.SaveChanges();
            }
            else
            {
                Visitas obj = new Visitas();
                var IUser = DbContext.Users.Find(UserId);
                var ICont = DbContext.Contenidos.Find(ContenidoId);
                obj.setCont(ICont);
                obj.setUser(IUser);
                obj.Contador = 1;
                obj.LastUpdate = DateTime.UtcNow;
                DbContext.Visitas.Add(obj);
                DbContext.SaveChanges();
            }
        }
    }
    public interface IVisitaRepository : IRepository<Visitas>
    {
        void UpdateVisitasByUser(int ContenidoId, string UserId);
        List<Visitas> GetVisitasByUserId(string Id);
    }
}
