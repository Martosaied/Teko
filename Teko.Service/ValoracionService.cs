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
        void AddValoracion(Valoraciones miVal);
        void SaveValoracion();
        double GetPromedioValoracionesByContenidos(int id);
        Valoraciones GetValoracionByUserAndContenido(int id, string userId);
        void DeleteValoracion(Valoraciones valoracionVieja);
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

        public void AddValoracion(Valoraciones miVal)
        {
            valoracionRepository.Add(miVal);
        }

        public void SaveValoracion()
        {
            unitOfWork.Commit();
        }

        public double GetPromedioValoracionesByContenidos(int id)
        {
            return valoracionRepository.GetValoracionesByContenido(id).Average();
        }

        public Valoraciones GetValoracionByUserAndContenido(int id, string userId)
        {
            Valoraciones ValoracionDeUser = valoracionRepository.GetValoracionByUserAndContenido(id, userId);
            return ValoracionDeUser;
        }

        public void DeleteValoracion(Valoraciones valoracionVieja)
        {
            valoracionRepository.Delete(valoracionVieja);
        }
    }
}
