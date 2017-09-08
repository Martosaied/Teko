using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Data.Infrastructure;
using Teko.Data.Repositories;
using Teko.Model;

namespace Teko.Service
{
    public interface IMateriaService
    {
        List<Materias> GetAll();
    }
    public class MateriaService : IMateriaService
    {
        private readonly IMateriaRepository materiaRepo;
        private readonly IUnitOfWork unitOfWork;

        public MateriaService(IMateriaRepository materiaRepo, IUnitOfWork unitOfWork)
        {
            this.materiaRepo = materiaRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<Materias> GetAll()
        {
            return materiaRepo.GetAll().ToList();
        }
    }
}
