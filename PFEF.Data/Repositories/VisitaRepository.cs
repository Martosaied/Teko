using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
{
    public class VisitaRepository : RepositoryBase<Visitas>,IVisitaRepository 
    {
        public VisitaRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public List<Visitas> ObtenerIntereses(int Id)
        {
            var Dia20Atras = DateTime.UtcNow.AddDays(-20);
            var UltimasVisitas = DbContext.Visitas.Where(x => x.User.Id == Id && x.LastUpdate > Dia20Atras).OrderByDescending(x => x.Contador).Take(5).ToList();

            return UltimasVisitas;
        }
        public void UpdateRecomendation(Contenidos cont, Usuarios User)
        {
            var result = DbContext.Visitas.Where(x => x.User.Id == User.Id && x.Contenido.Id == cont.Id).FirstOrDefault();

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
                var IUser = DbContext.Usuarios.Find(User.Id);
                var ICont = DbContext.Contenidos.Find(cont.Id);
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
        void UpdateRecomendation(Contenidos cont, Usuarios User);
        List<Visitas> ObtenerIntereses(int Id);
    }
}
