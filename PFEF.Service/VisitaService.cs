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
    public interface IVisitaService
    {
        List<Visitas> ObtenerIntereses(int id);
        void UpdateRecomendation(Contenidos cont, Usuarios User);
    }
    public class VisitaService : IVisitaService
    {
        private readonly IVisitaRepository visitaRepo;
        private readonly IUnitOfWork unitOfWork;

        public VisitaService(IVisitaRepository visitaRepo, IUnitOfWork unitOfWork)
        {
            this.visitaRepo = visitaRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<Visitas> ObtenerIntereses(int id)
        {
            return visitaRepo.ObtenerIntereses(id);
        }

        public void UpdateRecomendation(Contenidos cont, Usuarios User)
        {
            visitaRepo.UpdateRecomendation(cont, User);
        }
    }
}
