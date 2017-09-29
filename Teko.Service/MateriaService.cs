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
    public interface IMateriaService
    {
        List<Materias> GetAll();
        Dictionary<string, List<Materias>> GetAllByLetter();
    }
    public class MateriaService : IMateriaService
    {
        private readonly IMateriaRepository materiaRepo;
        private readonly IUnitOfWork unitOfWork;

        public MateriaService(IMateriaRepository materiaRepo, IUnitOfWork unitOfWork)
        {
            this.materiaRepo = materiaRepo;
            this.unitOfWork = unitOfWork;
        }

        public List<Materias> GetAll()
        {
            return materiaRepo.GetAll().ToList();
        }

        public Dictionary<string, List<Materias>> GetAllByLetter()
        {
            Dictionary<string, List<Materias>> DiccionarioADevolver = new Dictionary<string, List<Materias>>(); 
            var ListaMaterias = materiaRepo.GetAll();
            foreach (var materia  in ListaMaterias)
            {
                string LetraInicio = materia.Nombre.Substring(0, 1);
                if (DiccionarioADevolver.ContainsKey(LetraInicio))
                {
                    DiccionarioADevolver[LetraInicio].Add(materia);
                }else
                {
                    List<Materias> ListaNuevaMateria = new List<Materias>();
                    ListaNuevaMateria.Add(materia);
                    DiccionarioADevolver.Add(LetraInicio, ListaNuevaMateria);
                }
            }
            return DiccionarioADevolver;
        }
    }
}
