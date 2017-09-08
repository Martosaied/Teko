using Teko.Data.Infrastructure;
using Teko.Data.Repositories;
using Teko.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Service
{
    public interface IContenidoService
    {
        string BudgetSelect(string URL);
        Contenidos LastId();
        Contenidos[] _Filter(Contenidos Parameters, Contenidos[] Array);
        Contenidos[] GetContsByTitle(string title);
        Contenidos GetContById(int id);
        void CreateContenido(Contenidos cont);
        void UpdateDes(int id);
        void UpdatePop(int id);
        void SaveContenido();
        Contenidos[] ObtenerRecByCont(Contenidos cont);
        Dictionary<string, Contenidos[]> ObtenerRecByUser(List<Visitas> List);
        Contenidos[] Search(string Buscador);
        Contenidos[] GetContByTag(string Tag);
        Contenidos[] GetByRec();
        Contenidos[] GetByVal();
        Contenidos[] GetByPop();
        Contenidos[] GetByDes();
        Contenidos[] GetByEsc(int id);
        
    }

    public class ContenidoService : IContenidoService
    {
        private readonly IContenidoRepository contenidoRepo;
        private readonly IEscuelaRepository escuelaRepo;
        private readonly INivelEducativoRepository nivelRepo;
        private readonly ITiposContenidoRepository tipoRepo;
        private readonly IMateriaRepository materiaRepo;
        private readonly IUnitOfWork unitOfWork;

        public ContenidoService(IEscuelaRepository escuelaRepo,INivelEducativoRepository nivelRepo,ITiposContenidoRepository tipoRepo,IContenidoRepository contenidoRepository, IMateriaRepository materiaRepo, IUnitOfWork unitOfWork)
        {
            this.contenidoRepo = contenidoRepository;
            this.unitOfWork = unitOfWork;
            this.escuelaRepo = escuelaRepo;
            this.materiaRepo = materiaRepo;
            this.nivelRepo = nivelRepo;
            this.tipoRepo = tipoRepo;
        }

        #region IGadgetService Members

        public Contenidos[] _Filter(Contenidos Parameters, Contenidos[] Array)
        {
            if (Parameters.Profesor == null) Parameters.Profesor = "";
            var Lista = Array
               .Where(s => s.EscuelasId == Parameters.EscuelasId || Parameters.EscuelasId == 0)
               .Where(s => s.MateriasId == Parameters.MateriasId || Parameters.MateriasId == 0)
               .Where(s => s.TiposContenidosId == Parameters.TiposContenidosId || Parameters.TiposContenidosId == 0)
               //.Where(s => s.Escuelas.NivEduEscuela.Id == Parameters.Escuelas.NivEduEscuela.Id || Parameters.Escuelas.NivEduEscuela.Id == 0)
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

        public void UpdateDes(int id)
        {
            var cont = contenidoRepo.GetById(id);
            cont.IDes++;
            contenidoRepo.Update(cont);           
        }

        public void UpdatePop(int id)
        {
            var cont = contenidoRepo.GetById(id);
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

        public Contenidos[] GetByRec()
        {
            return contenidoRepo.GetContByRecent();
        }

        public Contenidos[] GetByVal()
        {
            return contenidoRepo.GetContByVal();
        }

        public Contenidos[] GetByPop()
        {
            return contenidoRepo.GetContByPop();
        }

        public Contenidos[] GetByDes()
        {
            return contenidoRepo.GetContByDes();
        }

        public Contenidos[] GetByEsc(int id)
        {
            return contenidoRepo.GetContByEsc(id);
        }

        public Dictionary<string, Contenidos[]> ObtenerRecByUser(List<Visitas> List)
        {
            return contenidoRepo.ObtenerRecGen(List);
        }

        public string BudgetSelect(string URL)
        {
            string ADevolver = "fa fa-file-o";
            URL = URL.Substring(URL.Length - 4, 4);
            switch (URL)
            {
                case "docx":
                case ".doc":
                    ADevolver = "fa fa-file-word-o";
                    break;
                case ".xls":
                case "xlsx":
                    ADevolver =  "fa fa-file-excel-o";
                    break;
                case ".ppt":
                case "pptx":
                    ADevolver = "fa fa-file-powerpoint-o";
                    break;
                case ".pdf":
                    ADevolver = "fa fa-file-pdf-o";
                    break;
                default:
                    break;
            }
            return ADevolver;
    }

        #endregion

    }
}
