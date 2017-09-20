using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Teko.Model
{
    public class Visitas
    {
        public int Id { get; set; }
        public virtual Usuarios User { get; set; }
        public virtual Contenidos Contenido { get; set; }
        public int Contador { get; set; }
        public DateTime LastUpdate { get; set; }
        public void setUser(Usuarios user)
        {
            User = user;
        }
        public void setCont(Contenidos cont)
        {
            Contenido = cont;
        }

    }
}