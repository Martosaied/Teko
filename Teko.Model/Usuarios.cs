using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Teko.Model
{
    public class Usuarios : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Usuarios> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RutaFoto { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaNacimiento { get; set; }

        public Nullable<int> InstitucionActualId { get; set; }

        public string ContenidosFav { get; set; }

        [ForeignKey("InstitucionActualId")] public virtual Escuelas InstitucionActual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contenidos> Contenidos { get; set; }

        public bool PerfilCompleto { get; set; }
    }
}
