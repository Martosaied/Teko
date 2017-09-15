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
    public interface IArchivoService
    {
        List<Archivos> GetArchivosByContenidoId(int id);
        List<string> GetURLToShowDocument(List<Archivos> ListaArchivos);
        void AddArchivo(Archivos Archivo);
        void SaveArchivo();
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

        public void AddArchivo(Archivos Archivo)
        {
            archivoRepo.Add(Archivo);
        }

        public List<Archivos> GetArchivosByContenidoId(int id)
        {
            return archivoRepo.GetArchivosByContenido(id);
        }
        public List<string> GetURLToShowDocument(List<Archivos> ListaArchivos)
        {
            List<string> Lista = new List<string>();
            foreach (var file in ListaArchivos)
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

        public void SaveArchivo()
        {
            unitOfWork.Commit();
        }
    }
}
