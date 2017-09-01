using PFEF.Data.Infrastructure;
using PFEF.Data.Repositories;
using PFEF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Service
{
    public interface IContenidoService
    {
        Contenidos LastId();
        Contenidos[] _Filter(Contenidos Parameters, Contenidos[] Array);
        Contenidos[] GetContsByTitle(string title);
        Contenidos GetContById(int id);
        void CreateContenido(Contenidos cont);
        void UpdateDes(Contenidos cont);
        void UpdatePop(Contenidos cont);
        void SaveContenido();
        Contenidos[] ObtenerRecByCont(Contenidos cont);
        Contenidos[] Search(string Buscador);
        Contenidos[] GetContByTag(string Tag);
    }

    public class ContenidoService : IContenidoService
    {
        private readonly IContenidoRepository contenidoRepo;
        private readonly IUnitOfWork unitOfWork;

        public ContenidoService(IContenidoRepository contenidoRepository, IUnitOfWork unitOfWork)
        {
            this.contenidoRepo = contenidoRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IGadgetService Members

        public Contenidos[] _Filter(Contenidos Parameters, Contenidos[] Array)
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

        public Contenidos[] GetContsByTitle(string Title)
        {
            switch (Title)
            {
                case "Mas recientes":
                    var result = contenidoRepo.GetContByRecent();
                    return result;
                case "Mas populares":
                    var result1 = contenidoRepo.GetContByPop();
                    return result1;
                case "Mas descargados":
                    var result2 = contenidoRepo.GetContByDes();
                    return result2;
                case "Mas valorados":
                    var result3 = contenidoRepo.GetContByVal();
                    return result3;
                default:
                    return contenidoRepo.GetAll().Cast<Contenidos>().ToArray();
                    
            }
        }

        public Contenidos GetContById(int id)
        {
            var cont = contenidoRepo.GetById(id);
            return cont;
        }

        public void CreateContenido(Contenidos cont)
        {
            contenidoRepo.Add(cont);
        }

        public void SaveContenido()
        {
            unitOfWork.Commit();
        }

        public void UpdateDes(Contenidos cont)
        {
            cont.IDes++;
            contenidoRepo.Update(cont);           
        }

        public void UpdatePop(Contenidos cont)
        {
            cont.IPop++;
            contenidoRepo.Update(cont);
        }

        public Contenidos[] ObtenerRecByCont(Contenidos cont)
        {
            return contenidoRepo.ObtenerRecByCont(cont);
        }

        public Contenidos[] Search(string Buscador)
        {
            if (Buscador == "")
            {
                return contenidoRepo.GetContByRecent();
            }
            else
            {
                List<string> keywordsL = new List<string>();
                string[] keywords = Buscador.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                keywordsL = keywords.ToList();
                int flag = 1;
                List<Contenidos> Lista = new List<Contenidos>();
                foreach (string item in keywordsL)
                {
                    if (flag == 1)
                    {
                        Lista = contenidoRepo._Searcher(item).ToList(); ;
                        flag = 0;
                    }
                    else
                    {
                        Lista = Lista.Where(s => s.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Descripcion.ToLower().Contains(item.ToLower()) || s.Profesor.ToLower().Contains(item.ToLower()) ||
                        s.Cursada.ToString().ToLower().Contains(item.ToLower()) || s.Usuarios.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Escuelas.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Escuelas.NivEduEscuela.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.TiposContenidos.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Materias.Nombre.ToLower().Contains(item.ToLower())).ToList();
                    }
                    
                }
                return Lista.ToArray();
            }
        }

        public Contenidos[] GetContByTag(string Tag)
        {
            return contenidoRepo.GetContenidosByTag(Tag);
        }

        public Contenidos LastId()
        {
            return contenidoRepo.LastId();
        }



        #endregion

    }
}
