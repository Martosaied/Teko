using Teko.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Model;
using Teko.Data.Repositories;

namespace Teko.Service
{
    public interface IValoracionService
    {
        void Agregar(Valoraciones miVal);
        void SaveVal();
        double GetPromVal(int id);
    }
    public class ValoracionService : IValoracionService
    {
        private readonly IValoracionRepository valoracionRepository;
        private readonly IUnitOfWork unitOfWork;

        public ValoracionService(IValoracionRepository valoracionRepository, IUnitOfWork unitOfWork)
        {
            this.valoracionRepository = valoracionRepository;
            this.unitOfWork = unitOfWork;
        }

        public void Agregar(Valoraciones miVal)
        {
            valoracionRepository.Add(miVal);
        }

        public void SaveVal()
        {
            unitOfWork.Commit();
        }

        public double GetPromVal(int id)
        {
            return valoracionRepository.GetByCont(id).Average();
        }
    }
}
