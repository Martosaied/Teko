using Teko.Data.Infrastructure;
using Teko.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Model;

namespace Teko.Service
{
    public interface INivelEducativoService
    {
        List<NivelesEducativos> GetAll();
    }
    public class NivelEducativoService : INivelEducativoService
    {
        private readonly INivelEducativoRepository nivelRepo;
        private readonly IUnitOfWork unitOfWork;

        public NivelEducativoService(INivelEducativoRepository nivelRepo, IUnitOfWork unitOfWork)
        {
            this.nivelRepo = nivelRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<NivelesEducativos> GetAll()
        {
            return nivelRepo.GetAll().ToList();
        }
    }
}
