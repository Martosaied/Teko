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
    public interface IEscuelaService 
    {
        List<Escuelas> GetEscuelasByNivel(int id);
        void CreateEscuela(Escuelas esc);
        void SaveEscuela();
    }
    public class EscuelaService : IEscuelaService
    {
        private readonly IEscuelaRepository escuelaRepo;
        private readonly IUnitOfWork unitOfWork;

        public EscuelaService(IEscuelaRepository escuelaRepo, IUnitOfWork unitOfWork)
        {
            this.escuelaRepo = escuelaRepo;
            this.unitOfWork = unitOfWork;
        }

        public void CreateEscuela(Escuelas esc)
        {
            escuelaRepo.Add(esc);
        }

        public List<Escuelas> GetEscuelasByNivel(int id)
        {
            return escuelaRepo.GetEscuelasByNivel(id);
        }

        public void SaveEscuela()
        {
            unitOfWork.Commit();
        }
    }
}
