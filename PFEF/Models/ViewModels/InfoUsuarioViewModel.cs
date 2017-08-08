using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFEF.Models.ViewModels
{
    public abstract class BaseUserViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Descripcion { get; set; }
        public Nullable<DateTime> FechaNacimiento { get; set; }
        public virtual Escuelas InstitucionActual { get; set; }
        public string RutaFoto { get; set; }
    }
    public class InfoUsuarioViewModel : BaseUserViewModel
    {
        [Key]public int Id { get; set; }

        [Required]new public string Nombre{ get; set; }
        [Required]new public string Apellido{ get; set; }
        [Required]new public string Descripcion{ get; set; }
        [Display(Name = "Foto de perfil")]new public string RutaFoto { get; set; }
        [Required][Display(Name = "Fecha de nacimiento")] new public string FechaNacimiento { get; set; }
        [Required][Display(Name = "¿En donde estudias actualmente?")] public int InstitucionActualId { get; set; }

        public List<Escuelas> dropEscuela { get; set; }

        public void setDropEsc()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                dropEscuela = db.Escuelas.ToList();
            }
        }
    }
    public class PerfilViewModel : BaseUserViewModel
    {
        public Dictionary<string, Contenidos[]> DictRecomendaciones { get; set; }
        
        

    }
}