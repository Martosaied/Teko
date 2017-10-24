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
    public interface IEscuelaService
    {
        List<Escuelas> GetEscuelasByNivel(int id);
        void CreateEscuela(Escuelas esc);
        void SaveEscuela();
        List<Escuelas> GetAll();
        Dictionary<string, List<Escuelas>> GetAllByLetter();
        Escuelas GetEscuelaById(int IdEscuela);
        Escuelas GetEscuelasByName(string Name);
    }
    public class EscuelaService : IEscuelaService
    {
        private readonly IEscuelaRepository escuelaRepo;
        private readonly IUnitOfWork unitOfWork;

        public EscuelaService(IEscuelaRepository escuelaRepo, IUnitOfWork unitOfWork)
        {
            this.escuelaRepo = escuelaRepo;
            this.unitOfWork = unitOfWork;
        }

        public Escuelas GetEscuelasByName(string Name)
        {
            var Lista = escuelaRepo.Get(parent => parent.Nombre == Name);
            return Lista;
        }
        public void CreateEscuela(Escuelas esc)
        {
            escuelaRepo.Add(esc);
        }

        public List<Escuelas> GetAll()
        {
            return escuelaRepo.GetAll().ToList();
        }

        public List<Escuelas> GetEscuelasByNivel(int id)
        {
            return escuelaRepo.GetEscuelasByNivel(id);
        }
        public Dictionary<string, List<Escuelas>> GetAllByLetter()
        {
            Dictionary<string, List<Escuelas>> DiccionarioADevolver = new Dictionary<string, List<Escuelas>>();
            var ListaEscuelas = escuelaRepo.GetAll().OrderBy(x => x.Nombre);
            foreach (var Escuela in ListaEscuelas)
            {
                string LetraInicio = Escuela.Nombre.Substring(0, 1);
                if (DiccionarioADevolver.ContainsKey(LetraInicio))
                {
                    DiccionarioADevolver[LetraInicio].Add(Escuela);
                }
                else
                {
                    List<Escuelas> ListaNuevaEscuela = new List<Escuelas>();
                    ListaNuevaEscuela.Add(Escuela);
                    DiccionarioADevolver.Add(LetraInicio, ListaNuevaEscuela);
                }
            }
            return DiccionarioADevolver;
        }
        public void SaveEscuela()
        {
            unitOfWork.Commit();
        }

        public Escuelas GetEscuelaById(int IdEscuela)
        {
            return escuelaRepo.GetById(IdEscuela);
        }
    }
}
