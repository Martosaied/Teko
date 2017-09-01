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
    public interface IArchivoService
    {
        List<Archivos> GetByContenido(int id);
    }
    public class ArchivoService : IArchivoService
    {
        private readonly IArchivosRepository archivoRepo;
        private readonly IUnitOfWork unitOfWork;

        public ArchivoService(IArchivosRepository archivoRepo, IUnitOfWork unitOfWork)
        {
            this.archivoRepo = archivoRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<Archivos> GetByContenido(int id)
        {
            return archivoRepo.GetByContenido(id);
        }
    }
}
