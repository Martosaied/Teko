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
        List<string> ObtenerURLArchivos(int id);
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
        public List<string> ObtenerURLArchivos(int id)
        {
            var Files = archivoRepo.GetByContenido(id);
            List<string> Lista = new List<string>();
            foreach (var file in Files)
            {
                string URL = file.Ruta.Substring(file.Ruta.Length - 4, 4);
                switch (URL)
                {
                    case ".pdf":
                        URL = "https://docs.google.com/viewer?url=https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta + "&embedded=true";
                        break;
                    case ".jpg":
                    case ".png":
                        URL = "https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta;
                        break;
                    default:
                        URL = "https://view.officeapps.live.com/op/embed.aspx?src=https://tekoteko.azurewebsites.net/Content/Uploads/" + file.Ruta;
                        break;
                }
                Lista.Add(URL);
            }

            return Lista;
        }
    }
}
