using PFEF.Data.Infrastructure;
using PFEF.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFEF.Service
{
    public interface INivelEducativoService
    {
        
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

    }
}
