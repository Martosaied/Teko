using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PFEF.ViewModels;
using Microsoft.AspNet.Identity;

namespace PFEF.Models.DataAccess
{
    public class ContenidosDA
    {
        public static ApplicationDbContext db = new ApplicationDbContext();
        public class Recomendaciones
        {
            public static bool UpdateRecomendation(Contenidos cont, Usuarios User)
            {
                var result = db.Visitas.Where(x => x.User.Id == User.Id && x.Contenido.Id == cont.Id).FirstOrDefault();

                if (result != null)
                {
                    db.Visitas.Attach(result);
                    if (result.LastUpdate < DateTime.UtcNow.AddDays(-20))
                    {
                        result.Contador++;
                    }
                    else
                    {
                        result.Contador = 1;
                    }
                    result.LastUpdate = DateTime.UtcNow;
                    db.SaveChanges();
                }
                else
                {
                    Visitas obj = new Visitas();
                    var IUser = db.Usuarios.Find(User.Id);
                    var ICont = db.Contenidos.Find(cont.Id);
                    obj.setCont(ICont);
                    obj.setUser(IUser);
                    obj.Contador = 1;
                    obj.LastUpdate = DateTime.UtcNow;
                    db.Visitas.Add(obj);
                    db.SaveChanges();
                }
                return true;

            }
            public static Contenidos[] ObtenerRec(Contenidos cont)
            {
                    var result = db.Contenidos.Where(x => x.Materias.Id == cont.MateriasId
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
            public static List<Visitas> ObtenerIntereses(int Id)
            {
                Dictionary<string, int> Intereses = new Dictionary<string, int>();
                var IUser = db.Usuarios.Find(Id);
                var Dia20Atras = DateTime.UtcNow.AddDays(-20);
                var UltimasVisitas = db.Visitas.Where(x => x.User.Id == IUser.Id && x.LastUpdate > Dia20Atras).OrderByDescending(x => x.Contador).Take(5).ToList();


                return UltimasVisitas;
            }
            public static Dictionary<string, Contenidos[]> ObtenerRec(List<Visitas> Visitas)
            {
                Dictionary<string, Contenidos[]> Recomendaciones = new Dictionary<string, Contenidos[]>();
                int i = 0;
                foreach (var item in Visitas)
                {                 
                    var result = db.Contenidos.Where(x => x.Materias.Id == item.Contenido.MateriasId
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
        }

        public static Contenidos[] _Searcher(string Buscador)
        {
            List<string> keywordsL = new List<string>();
            MuestraViewModel Buscado = new MuestraViewModel();
            if (Buscador == "")
            {
                Buscado.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.Id).ToArray();
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
                        Buscado.ListaAMostrar = db.Contenidos.Where(s => s.Nombre.Contains(item) ||
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
        public static Contenidos[] _SearcherByTag(string Buscador)
        {
            MuestraViewModel Buscado = new MuestraViewModel()
            {
                ListaAMostrar = db.Contenidos.Where(s => s.Nombre.Contains(Buscador) ||
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
        public static Contenidos[] _Filter(MuestraViewModel Parameters, Contenidos[] Array)
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
        public static void UpdateIdes(bool DesOVis, int Id)
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
    }
}