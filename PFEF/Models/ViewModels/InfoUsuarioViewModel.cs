﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFEF.Models.ViewModels
{
    public class InfoUsuarioViewModel 
    {
        [Key]public int Id { get; set; }

        [Required]public string Nombre{ get; set; }
        [Required]public string Apellido{ get; set; }
        [Required]public string Descripcion{ get; set; }
        [Display(Name = "Foto de perfil")]public string RutaFoto { get; set; }
        [Required][Display(Name = "Fecha de nacimiento")] public string FechaNacimiento { get; set; }
        [Required][Display(Name = "¿En donde estudias actualmente?")] public int InstitucionActualId { get; set; }

        public List<Escuelas> dropEscuela { get; set; }
    }
    public class PerfilViewModel
    {
        public Contenidos[] DictRecomendaciones { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string RutaFoto { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaNacimiento { get; set; }
        public virtual Escuelas InstitucionActual { get; set; }
    }
}