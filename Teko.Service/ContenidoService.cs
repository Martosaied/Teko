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
        string SelectBadget(string URL);
        int LastId();
        Contenidos[] FiltrarContenidos(Contenidos Parameters, Contenidos[] Array);
        Contenidos[] GetContsByTitle(string title);
        Contenidos GetContenidoById(int id);
        void CreateContenido(Contenidos cont);
        void UpdateContenidoDescargas(int id);
        void UpdateContenidoPopularidad(int id);
        void SaveContenido();
        Contenidos[] GetRecomendacionesByContenido(Contenidos cont);
        Dictionary<string, Contenidos[]> GetRecomendacionesByVisitas(List<Visitas> List);
        Contenidos[] GetContenidosByKeywords(string Buscador);
        Contenidos[] GetContenidosByTag(string Tag);
        Contenidos[] GetContenidosOrderRecent();
        Contenidos[] GetContenidosOrderValoracion();
        Contenidos[] GetContenidosOrderPopular();
        Contenidos[] GetContenidosOrderDescargas();
        Contenidos[] GetContenidosByEscuela(int id);
        void BajaLogica(int ContenidoId);


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

        public Contenidos[] FiltrarContenidos(Contenidos Parameters, Contenidos[] Array)
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

        public void BajaLogica(int  ContenidoId)
        {
            Contenidos Contenido = contenidoRepo.GetById(ContenidoId);
            Contenido.IsDeleted = true;
            contenidoRepo.Update(Contenido);
        }

        public Contenidos[] GetContsByTitle(string Title)
        {
            switch (Title)
            {
                case "Mas recientes":
                    var result = contenidoRepo.GetContenidosOrderRecent();
                    return result;
                case "Mas populares":
                    var result1 = contenidoRepo.GetContenidosOrderPopular();
                    return result1;
                case "Mas descargados":
                    var result2 = contenidoRepo.GetContenidosOrderDescargas();
                    return result2;
                case "Mas valorados":
                    var result3 = contenidoRepo.GetContenidosOrderValoracion();
                    return result3;
                default:
                    return contenidoRepo.GetAll().Cast<Contenidos>().ToArray();
                    
            }
        }

        public Contenidos GetContenidoById(int id)
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

        public void UpdateContenidoDescargas(int id)
        {
            var cont = contenidoRepo.GetById(id);
            cont.IDes++;
            contenidoRepo.Update(cont);           
        }

        public void UpdateContenidoPopularidad(int id)
        {
            var cont = contenidoRepo.GetById(id);
            cont.IPop++;
            contenidoRepo.Update(cont);
        }

        public Contenidos[] GetRecomendacionesByContenido(Contenidos cont)
        {
            return contenidoRepo.GetRecomendacionesByContenido(cont);
        }

        public Contenidos[] GetContenidosByKeywords(string Buscador)
        {
            if (Buscador == "")
            {
                return contenidoRepo.GetContenidosOrderRecent();
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
                        Lista = contenidoRepo.GetContenidosByKeywords(item).ToList(); ;
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

        public Contenidos[] GetContenidosByTag(string Tag)
        {
            return contenidoRepo.GetContenidosByTag(Tag);
        }

        public int LastId()
        {
            return contenidoRepo.LastId();
        }

        public Contenidos[] GetContenidosOrderRecent()
        {
            return contenidoRepo.GetContenidosOrderRecent();
        }

        public Contenidos[] GetContenidosOrderValoracion()
        {
            return contenidoRepo.GetContenidosOrderValoracion();
        }

        public Contenidos[] GetContenidosOrderPopular()
        {
            return contenidoRepo.GetContenidosOrderPopular();
        }

        public Contenidos[] GetContenidosOrderDescargas()
        {
            return contenidoRepo.GetContenidosOrderDescargas();
        }

        public Contenidos[] GetContenidosByEscuela(int id)
        {
            return contenidoRepo.GetContenidosByEscuela(id);
        }

        public Dictionary<string, Contenidos[]> GetRecomendacionesByVisitas(List<Visitas> List)
        {
            return contenidoRepo.GetRecomendacionesByVisitas(List);
        }

        public string SelectBadget(string URL)
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
