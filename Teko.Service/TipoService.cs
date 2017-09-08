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
    public interface ITipoService
    {
        List<TiposContenidos> GetAll();
    }
    public class TipoService : ITipoService
    {
        private readonly ITiposContenidoRepository tipoRepo;
        private readonly IUnitOfWork unitOfWork;

        public TipoService(ITiposContenidoRepository tipoRepo, IUnitOfWork unitOfWork)
        {
            this.tipoRepo = tipoRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<TiposContenidos> GetAll()
        {
            return tipoRepo.GetAll().ToList();
        }
    }
}
