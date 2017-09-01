//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PFEF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Usuarios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuarios()
        {
            this.Contenidos = new HashSet<Contenidos>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RutaFoto { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaNacimiento { get; set; }

        public Nullable<int> InstitucionActualId { get; set; }

        public string ContenidosFav { get; set; }

        [ForeignKey("InstitucionActualId")]public virtual Escuelas InstitucionActual { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Contenidos> Contenidos { get; set; }

        public bool PerfilCompleto { get; set; }

    }
}
