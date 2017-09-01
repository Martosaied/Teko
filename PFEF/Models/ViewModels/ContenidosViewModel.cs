using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PFEF.Models;

namespace PFEF.ViewModels
{
    public abstract class DropsCharger
    {
        
    }

    public abstract class BaseContentViewModel
    {
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string Descripcion { get; set; }
        public string Cursada { get; set; }
        public int UsuariosId { get; set; }
        public int EscuelasId { get; set; }
        public int MateriasId { get; set; }
        public int TiposContenidosId { get; set; }
        public int NivelesEducativosId { get; set; }

        public List<Escuelas> dropEscuela { get; set; }
        public List<Materias> dropMateria { get; set; }
        public List<TiposContenidos> dropTipoContenido { get; set; }
        public List<NivelesEducativos> dropNivelEducativo { get; set; }

        public void ChargeDrops()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                dropEscuela = db.Escuelas.ToList();
                dropMateria = db.Materias.ToList();
                dropTipoContenido = db.TiposContenidos.ToList();
                dropNivelEducativo = db.NivelesEducativos.ToList();
            }
        }
    }

    public class BuscadorViewModel
    {
        public string KeyWords { get; set; }
    }
    public class SubirViewModel : BaseContentViewModel
    {
        [Required]
        public new string Nombre { get; set; }
        [Required]
        new public string Descripcion { get; set; }
        public string Ruta { get; set; }
        [Required]
        new public string Profesor { get; set; }
        [Required]
        new public string Cursada { get; set; }
        [Required]
        new public int UsuariosId { get; set; }
        [Required]
        new public int EscuelasId { get; set; }
        [Required]
        new public int MateriasId { get; set; }
        [Required]
        new public int NivelesEducativosId { get; set; }
        [Required]
        new public int TiposContenidosId { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public int NivNuevaEsc { get; set; }
        public string NuevaEsc { get; set; }

    }
    public class MuestraViewModel : BaseContentViewModel
    {
       
        public int Pagina { get; set; }
        public string Title { get; set; }
        public Contenidos[] ListaAMostrar { get; set; }

        //Con esto llenamos los drop en un objeto y mostrar las listas desde un objeto creado sin la necesidad de utilizar viewbags

    }
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> Rutas { get; set; }
        public string Profesor { get; set; }
        public int Cursada { get; set; }
        public int UsuariosId { get; set; }
        public int IPop { get; set; }
        public int IDes { get; set; }
        public DateTime FechaSubida { get; set; }
        public string ValoracionPromedio
        {
            get;set;
        }

        public virtual Usuarios Usuarios { get; set; }
        public virtual Escuelas Escuelas { get; set; }
        public virtual Materias Materias { get; set; }
        public virtual TiposContenidos TiposContenidos { get; set; }

        public Contenidos[] Recomendaciones { get; set; }

    }
}