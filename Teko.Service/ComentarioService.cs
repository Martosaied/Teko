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
    public class ComentarioService : IComentarioService
    {
        private readonly IComentarioRepository comentarioRepo;
        private readonly IUnitOfWork unitOfWork;
        
        public ComentarioService(IComentarioRepository comentarioRepo, IUnitOfWork unitOfWork)
        {
            this.comentarioRepo = comentarioRepo;
            this.unitOfWork = unitOfWork;
        }

        public void AgregarComentario(Comentarios ComentarioAAgregar)
        {
            comentarioRepo.Add(ComentarioAAgregar);
        }

        public List<Comentarios> GetByContenidos(int Id)
        {
            var ListaComentarios = comentarioRepo.GetByContenido(Id);
            if(ListaComentarios == null)
            {
                return ListaComentarios = new List<Comentarios>();
            }
            else
            {
                return ListaComentarios;
            }
        }

        public void SaveComentario()
        {
            unitOfWork.Commit();
        }
    }
    public interface IComentarioService
    {
        List<Comentarios> GetByContenidos(int Id);
        void AgregarComentario(Comentarios ComentarioAAgregar);
        void SaveComentario();
    }
}
