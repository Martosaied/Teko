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
        Contenidos[] GetConts();
        Contenidos[] GetContsByTitle(string title);
        Contenidos GetContById(int id);
        void CreateContenido(Contenidos cont);
        void SaveGadget();
    }

    public class ContenidoService : IContenidoService
    {
        private readonly IContenidoRepository contenidoRepo;
        private readonly IUnitOfWork unitOfWork;

        public ContenidoService(IContenidoRepository gadgetsRepository, IUnitOfWork unitOfWork)
        {
            this.contenidoRepo = gadgetsRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IGadgetService Members

        public Contenidos[] GetConts()
        {
            var conts = contenidoRepo.GetAll();
            return conts;
        }
        public Contenidos[] GetContsByTitle(string Title)
        {
            switch (Title)
            {
                case "Mas recientes":
                    var result = contenidoRepo.GetContByRecent();
                    ViewBag.Title = Title;
                    return result;
                case "Mas populares":
                    var result1 = contenidoRepo.GetContByPop();
                    ViewBag.Title = Title;
                    return result1;
                case "Mas descargados":
                    var result2 = contenidoRepo.GetContByDes();
                    ViewBag.Title = Title;
                    return result2;
                case "Mas valorados":
                    var result3 = contenidoRepo.GetContByVal();
                    ViewBag.Title = Title;
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

        public void SaveGadget()
        {
            unitOfWork.Commit();
        }

        #endregion

    }
}
