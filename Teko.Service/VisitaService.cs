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
    public interface IVisitaService
    {
        List<Visitas> GetVisitasByUser (string id);
        void UpdateVisitasByUser(int ContenidoId, string UserId);
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

        public List<Visitas> GetVisitasByUser(string id)
        {
            return visitaRepo.GetVisitasByUserId(id);
        }

        public void UpdateVisitasByUser(int ContenidoId, string UserId)
        {
            visitaRepo.UpdateVisitasByUser(ContenidoId, UserId);
        }
    }
}
