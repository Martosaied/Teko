using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PFEF.Models;

namespace PFEF.ViewModels
{
    public class DropsCharger
    {
        public List<Escuelas> dropEscuela { get; set; }
        public List<Materias> dropMateria { get; set; }
        public List<TiposContenidos> dropTipoContenido { get; set; }
        public List<NivelesEducativos> dropNivelEducativo { get; set; }
    }
    public class DescrgarViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        public string Profesor { get; set; }
        public int Cursada { get; set; }
        public int UsuariosId { get; set; }
        public int IPop { get; set; }
        public int IDes { get; set; }
        public DateTime FechaSubida { get; set; }

        public virtual Usuarios Usuarios { get; set; }
        public virtual Escuelas Escuelas { get; set; }
        public virtual Materias Materias { get; set; }
        public virtual NivelesEducativos NivelesEducativos { get; set; }
        public virtual TiposContenidos TiposContenidos { get; set; }

        [Required]
        public string IsAuth { get; set; }
    }
    public class BuscadorViewModel
    {
        public string KeyWords { get; set; }
    }
    public class SubirViewModel : DropsCharger
    {


        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        [Required]
        public string Profesor { get; set; }
        [Required]
        public string Cursada { get; set; }
            [Required]
            public int UsuariosId { get; set; }
        [Required]
        public int EscuelasId { get; set; }
        [Required]
        public int MateriasId { get; set; }
        [Required]
        public int NivelesEducativosId { get; set; }
        [Required]
        public int TiposContenidosId { get; set; }
    }
    public class MuestraViewModel : DropsCharger
    {
        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string Cursada { get; set; }
        public int IdUsuario { get; set; }
        public int IdEscuela { get; set; }
        public int IdMateria { get; set; }
        public int IdTipoContenido { get; set; }
        public int IdNivelEducativo { get; set; }
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
        public string Ruta { get; set; }
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
        public virtual NivelesEducativos NivelesEducativos { get; set; }
        public virtual TiposContenidos TiposContenidos { get; set; }

    }
}