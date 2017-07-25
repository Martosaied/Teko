using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFEF.Models
{
    public class BaseIntereses
    {
        public virtual Usuarios IdUsuario { get; set; }
        public int Contador { get; set; }
        public void setUser(Usuarios user)
        {
            IdUsuario = user;
        }
    }
    public class InteresesMaterias : BaseIntereses
    {
        public int Id { get; set; }
        public virtual Materias IdMateria { get; set; }

        public void setMateria(Materias mat)
        {
            IdMateria = mat;
        }
    }
    public class InteresesEscuelas : BaseIntereses
    {
        public int Id { get; set; }
        public virtual Escuelas IdEscuela { get; set; }

        public void setEscuela(Escuelas esc)
        {
            IdEscuela = esc;
        }

    }
    public class InteresesProfesores : BaseIntereses
    {
        public int Id { get; set; }
        public string Profesor { get; set; }
    }
}