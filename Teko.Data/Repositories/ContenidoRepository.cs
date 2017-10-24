using Microsoft.AspNet.Identity;
using Teko.Data.Infrastructure;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Data.Repositories
{
    public class ContenidoRepository : RepositoryBase<Contenidos>, IContenidoRepository
    {
        public ContenidoRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        
        public override IEnumerable<Contenidos> GetAll()
        {
            var result = DbContext.Contenidos.ToArray();
            return result;
        }
        public Contenidos[] GetContenidosOrderDescargas()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.IDes).ToArray();
        }

        public Contenidos[] GetContenidosByEscuela(int id)
        {
            return DbContext.Contenidos.Where(x => x.Escuelas.Id == id).ToArray();
        }

        public Contenidos[] GetContenidosOrderPopular()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.IPop).ToArray();
        }

        public Contenidos[] GetContenidosOrderRecent()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.Id).ToArray();
        }

        public Contenidos[] GetContenidoByUserId(string id)
        {
            return DbContext.Contenidos.Where(x => x.Usuarios.Id == id).ToArray();
        }

        public Contenidos[] GetContenidosOrderValoracion()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.ValoracionPromedio).ToArray();
        }

        public Contenidos[] GetRecomendacionesByContenido(Contenidos cont)
        {
            var result = DbContext.Contenidos.Where(x => x.Materias.Id == cont.MateriasId
                                         || x.Escuelas.Id == cont.EscuelasId
                                         || x.Profesor.Contains(cont.Profesor))
                        .OrderByDescending(x => x.EscuelasId == cont.EscuelasId
                                                             && x.Materias.Id == cont.MateriasId
                                                             && x.Profesor.Contains(cont.Profesor))
                        .ThenByDescending(x => x.Escuelas.Id == cont.EscuelasId
                                                    && x.Materias.Id == cont.MateriasId
                                                    || x.Materias.Id == cont.MateriasId
                                                    && x.Profesor.Contains(cont.Profesor)
                                                    || x.Escuelas.Id == cont.EscuelasId
                                                    && x.Profesor.Contains(cont.Profesor))
                       .ThenByDescending(x => x.Materias.Id == cont.MateriasId
                                                   || x.Escuelas.Id == cont.EscuelasId
                                                   || x.Profesor.Contains(cont.Profesor))
                       .ThenByDescending(x => x.Cursada).ToList();

            var Recomendaciones = result.Where(x => x.Id != cont.Id).ToArray();

            return Recomendaciones;
        }
        public Dictionary<string, Contenidos[]> GetRecomendacionesByVisitas(List<Visitas> Visitas)
        {
            Dictionary<string, Contenidos[]> Recomendaciones = new Dictionary<string, Contenidos[]>();
            int i = 0;
            foreach (var item in Visitas)
            {
                var result = DbContext.Contenidos.Where(x => x.Materias.Id == item.Contenido.MateriasId
                                     || x.Escuelas.Id == item.Contenido.EscuelasId
                                     || x.Profesor.Contains(item.Contenido.Profesor))
                    .OrderByDescending(x => x.EscuelasId == item.Contenido.EscuelasId
                                                         && x.Materias.Id == item.Contenido.MateriasId
                                                         && x.Profesor.Contains(item.Contenido.Profesor))
                    .ThenByDescending(x => x.Escuelas.Id == item.Contenido.EscuelasId
                                                && x.Materias.Id == item.Contenido.MateriasId
                                                || x.Materias.Id == item.Contenido.MateriasId
                                                && x.Profesor.Contains(item.Contenido.Profesor)
                                                || x.Escuelas.Id == item.Contenido.EscuelasId
                                                && x.Profesor.Contains(item.Contenido.Profesor))
                   .ThenByDescending(x => x.Materias.Id == item.Contenido.MateriasId
                                               || x.Escuelas.Id == item.Contenido.EscuelasId
                                               || x.Profesor.Contains(item.Contenido.Profesor))
                   .ThenByDescending(x => x.Cursada).ToList();

                result.Remove(item.Contenido);
                Recomendaciones[item.Contenido.Nombre] = result.ToArray();
                i++;

            }

            return Recomendaciones;
        }      
        public Contenidos[] GetContenidosByKeywords(string item)
        {
            return DbContext.Contenidos.Where(s => s.Nombre.Contains(item) ||
                        s.Descripcion.Contains(item) || s.Profesor.Contains(item) ||
                        s.Cursada.ToString().Contains(item) || s.Usuarios.Nombre.Contains(item) ||
                        s.Escuelas.Nombre.Contains(item) ||
                        s.Escuelas.NivEduEscuela.Nombre.Contains(item) ||
                        s.TiposContenidos.Nombre.Contains(item) ||
                        s.Materias.Nombre.Contains(item)
                        ).ToArray();
        }

        public Contenidos[] GetContenidosByTag(string Buscador)
        {

            return DbContext.Contenidos.Where(s => s.Nombre.Contains(Buscador) ||
                        s.Descripcion.Contains(Buscador) || s.Profesor.Contains(Buscador) ||
                        s.Cursada.ToString().Contains(Buscador) || s.Usuarios.Nombre.Contains(Buscador) ||
                        s.Escuelas.Nombre.Contains(Buscador) ||
                        s.Escuelas.NivEduEscuela.Nombre.Contains(Buscador) ||
                        s.TiposContenidos.Nombre.Contains(Buscador) ||
                        s.Materias.Nombre.Contains(Buscador)
                    ).ToArray();
        }

        public int LastId()
        {
            return DbContext.Contenidos.OrderByDescending(p => p.Id).Select(x => x.Id).FirstOrDefault();
        }

        public void BajaLogicaRawQuery(int idContenido)
        {
            DbContext.Database.ExecuteSqlCommand("UPDATE Contenidos SET IsDeleted = 'True' WHERE Id = "+ idContenido);
        }
    }

    public interface IContenidoRepository : IRepository<Contenidos>
    {
        void BajaLogicaRawQuery(int idContenido);
        Dictionary<string, Contenidos[]> GetRecomendacionesByVisitas(List<Visitas> Visitas);
        Contenidos[] GetRecomendacionesByContenido(Contenidos cont);
        Contenidos[] GetContenidosByTag(string Tag);
        Contenidos[] GetContenidosByKeywords(string Buscador);
        Contenidos[] GetContenidosOrderRecent();
        Contenidos[] GetContenidosOrderPopular();
        Contenidos[] GetContenidosOrderDescargas();
        Contenidos[] GetContenidosOrderValoracion();
        Contenidos[] GetContenidosByEscuela(int IdEsc);
        Contenidos[] GetContenidoByUserId(string Id);
        int LastId();
    }
}

