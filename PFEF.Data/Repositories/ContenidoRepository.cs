using Microsoft.AspNet.Identity;
using PFEF.Data.Infrastructure;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Data.Repositories
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
        public Contenidos[] GetContByDes()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.IDes).ToArray();
        }

        public Contenidos[] GetContByEsc(int id)
        {
            return DbContext.Contenidos.Where(x => x.Escuelas.Id == id).ToArray();
        }

        public Contenidos[] GetContByPop()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.IPop).ToArray();
        }

        public Contenidos[] GetContByRecent()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.Id).ToArray();
        }

        public Contenidos[] GetContByUser(int id)
        {
            return DbContext.Contenidos.Where(x => x.Usuarios.Id == id).ToArray();
        }

        public Contenidos[] GetContByVal()
        {
            return DbContext.Contenidos.OrderByDescending(x => x.ValoracionPromedio).ToArray();
        }

        public Contenidos[] ObtenerRecByCont(Contenidos cont)
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
        public Dictionary<string, Contenidos[]> ObtenerRecGen(List<Visitas> Visitas)
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

        public void UpdateIdes(bool DesOVis, int Id)
        {
            var Cont = db.Contenidos.Find(Id);
            if (DesOVis)
            {
                Cont.IPop++;
                db.Contenidos.Attach(Cont);
                db.Entry(Cont).Property(x => x.IPop).IsModified = true;
                db.SaveChanges();
            }
            else
            {
                Cont.IDes++;
                db.Contenidos.Attach(Cont);
                db.Entry(Cont).Property(x => x.IDes).IsModified = true;
                db.SaveChanges();
            }
        }

        public Contenidos[] _Filter(MuestraViewModel Parameters, Contenidos[] Array)
        {
            if (Parameters.Profesor == null) Parameters.Profesor = "";
            var Lista = Array
               .Where(s => s.EscuelasId == Parameters.EscuelasId || Parameters.EscuelasId == 0)
               .Where(s => s.MateriasId == Parameters.MateriasId || Parameters.MateriasId == 0)
               .Where(s => s.TiposContenidosId == Parameters.TiposContenidosId || Parameters.TiposContenidosId == 0)
               .Where(s => s.Escuelas.NivEduEscuela.Id == Parameters.NivelesEducativosId || Parameters.NivelesEducativosId == 0)
               .Where(s => s.Profesor.ToLower().Contains(Parameters.Profesor.ToLower()) || Parameters.Profesor == "")
               .ToArray();
            return Lista;
        }

        public Contenidos[] _Searcher(string Buscador)
        {
            List<string> keywordsL = new List<string>();
            MuestraViewModel Buscado = new MuestraViewModel();
            if (Buscador == "")
            {
                Buscado.ListaAMostrar = DbContext.Contenidos.OrderByDescending(x => x.Id).ToArray();
                return Buscado.ListaAMostrar;
            }
            else
            {
                string[] keywords = Buscador.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                keywordsL = keywords.ToList();
                int flag = 1;
                foreach (string item in keywordsL)
                {
                    if (flag == 1)
                    {
                        Buscado.ListaAMostrar = DbContext.Contenidos.Where(s => s.Nombre.Contains(item) ||
                        s.Descripcion.Contains(item) || s.Profesor.Contains(item) ||
                        s.Cursada.ToString().Contains(item) || s.Usuarios.Nombre.Contains(item) ||
                        s.Escuelas.Nombre.Contains(item) ||
                        s.Escuelas.NivEduEscuela.Nombre.Contains(item) ||
                        s.TiposContenidos.Nombre.Contains(item) ||
                        s.Materias.Nombre.Contains(item)
                        ).ToArray();
                        flag = 0;
                    }
                    else
                    {
                        Buscado.ListaAMostrar = Buscado.ListaAMostrar.Where(s => s.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Descripcion.ToLower().Contains(item.ToLower()) || s.Profesor.ToLower().Contains(item.ToLower()) ||
                        s.Cursada.ToString().ToLower().Contains(item.ToLower()) || s.Usuarios.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Escuelas.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Escuelas.NivEduEscuela.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.TiposContenidos.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Materias.Nombre.ToLower().Contains(item.ToLower())).ToArray();
                    }
                }
                return Buscado.ListaAMostrar;
            }
        }

        public Contenidos[] _SearcherByTag(string Buscador)
        {
            MuestraViewModel Buscado = new MuestraViewModel()
            {
                ListaAMostrar = DbContext.Contenidos.Where(s => s.Nombre.Contains(Buscador) ||
                            s.Descripcion.Contains(Buscador) || s.Profesor.Contains(Buscador) ||
                            s.Cursada.ToString().Contains(Buscador) || s.Usuarios.Nombre.Contains(Buscador) ||
                            s.Escuelas.Nombre.Contains(Buscador) ||
                            s.Escuelas.NivEduEscuela.Nombre.Contains(Buscador) ||
                            s.TiposContenidos.Nombre.Contains(Buscador) ||
                            s.Materias.Nombre.Contains(Buscador)
                        ).ToArray()
            };
            return Buscado.ListaAMostrar;
        }

    }

    public interface IContenidoRepository : IRepository<Contenidos>
    {
        Dictionary<string, Contenidos[]> ObtenerRecGen(List<Visitas> Visitas);
        Contenidos[] ObtenerRecByCont(Contenidos cont);
        Contenidos[] _Filter(MuestraViewModel Parameters, Contenidos[] Array);
        Contenidos[] _SearcherByTag(string Buscador);
        Contenidos[] _Searcher(string Buscador);
        void UpdateIdes(bool DesOVis, int Id);
        Contenidos[] GetContByRecent();
        Contenidos[] GetContByPop();
        Contenidos[] GetContByDes();
        Contenidos[] GetContByVal();
        Contenidos[] GetContByEsc(int IdEsc);
        Contenidos[] GetContByUser(int Id);

    }
}

