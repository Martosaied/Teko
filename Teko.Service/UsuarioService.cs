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
    public interface IUsuarioService
    {
        Usuarios GetById(string id);
        void Modificar(Usuarios mappedUser);
        void SaveUser();
    }
    public class UsuarioService: IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IUnitOfWork unitOfWork;

        public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
        {
            this.usuarioRepository = usuarioRepository;
            this.unitOfWork = unitOfWork;
        }

        public Usuarios GetById(string id)
        {
            return usuarioRepository.GetById(id);
        }

        public void Modificar(Usuarios mappedUser)
        {
            usuarioRepository.Update(mappedUser);
        }

        public void SaveUser()
        {
            unitOfWork.Commit();
        }
    }
}
