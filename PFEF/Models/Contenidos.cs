//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PFEF.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    public partial class Contenidos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        public string Profesor { get; set; }
        public int Cursada { get; set; }
        public int UsuariosId { get; set; }
        public Nullable<int> EscuelasId { get; set; }
        public Nullable<int> MateriasId { get; set; }
        public Nullable<int> NivelesEducativosId { get; set; }
        public Nullable<int> TiposContenidosId { get; set; }
        public Nullable<int> IPop { get; set; }
        public Nullable<int> IDes { get; set; }
        public Nullable<System.DateTime> FechaSubida { get; set; }
        public double ValoracionPromedio { get; set; }

        [ForeignKey("UsuariosId")]
        public virtual Usuarios Usuarios { get; set; }
        public virtual Escuelas Escuelas { get; set; }
        public virtual Materias Materias { get; set; }
        public virtual NivelesEducativos NivelesEducativos { get; set; }
        public virtual TiposContenidos TiposContenidos { get; set; }
    }
}
