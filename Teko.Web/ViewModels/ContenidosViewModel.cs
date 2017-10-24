using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Teko.Models;
using Teko.Model;
using Teko.Service;
using Teko.Web.Extensions;
using Teko.Web.ViewModels;

namespace Teko.ViewModels
{ 

    public class BaseContentViewModel
    {
        protected IMateriaService materiaService;
        protected INivelEducativoService nivelService;
        protected ITipoService tipoService;
        protected IEscuelaService escuelaService;

        public BaseContentViewModel(IEscuelaService escuelaService, ITipoService tipoService, INivelEducativoService nivelService, IMateriaService materiaService)
        {
            this.escuelaService = escuelaService;
            this.tipoService = tipoService;
            this.materiaService = materiaService;
            this.nivelService = nivelService;
        }

        public void SetServices(IEscuelaService escuelaService, ITipoService tipoService, INivelEducativoService nivelService, IMateriaService materiaService)
        {
            this.escuelaService = escuelaService;
            this.tipoService = tipoService;
            this.materiaService = materiaService;
            this.nivelService = nivelService;
        }

        public BaseContentViewModel()
        {

        }

        public string Nombre { get; set; }
        public string Profesor { get; set; }
        public string Descripcion { get; set; }
        public string Cursada { get; set; }
        public string UsuariosId { get; set; }
        public int EscuelasId { get; set; }
        public int MateriasId { get; set; }
        public int TiposContenidosId { get; set; }
        public int NivelesEducativosId { get; set; }
        public string Badget { get; set; }
        public List<Escuelas> dropEscuela {
            get
            {
                return (dropEscuela == null) ? new List<Escuelas>() : dropEscuela;
            }
            set
            {
                dropEscuela = value;
            }
        }
        public List<Escuelas> getEscuelas()
        {
            try
            {
                var Lista = escuelaService.GetAll().ToList();
                Lista.Add(new Escuelas { Id = -1, Nombre = "----------" });
                Lista.Add(new Escuelas { Id = 0, Nombre = "Otra escuela" });
                Lista = Lista.OrderBy(x => x.Id).ToList();
                return Lista;
            }
            catch
            {
                var Lista = new List<Escuelas>();
                Lista.Add(new Escuelas { Id = -1, Nombre = "----------" });
                return Lista;
            }
        }
        public List<Materias> dropMateria
        {
            get
            {
                var Lista = materiaService.GetAll().ToList();
                Lista.Add(new Materias { Id = -1, Nombre = "----------" });
                Lista = Lista.OrderBy(x => x.Id).ToList();
                return Lista;
            }
            set
            {
            }
        }
        public List<TiposContenidos> dropTipoContenido
        {
            get
            {
                var Lista = tipoService.GetAll().ToList();
                Lista.Add(new TiposContenidos { Id = -1, Nombre = "----------" });
                Lista = Lista.OrderBy(x => x.Id).ToList();
                return Lista;
            }
            set
            {
            }
        }
        public List<NivelesEducativos> dropNivelEducativo
        {
            get
            {
                var Lista = nivelService.GetAll().ToList();
                Lista.Add(new NivelesEducativos{ Id = -1, Nombre = "----------" });
                Lista = Lista.OrderBy(x => x.Id).ToList();
                return Lista;
            }
            set
            {
            }
        }
    }

    public class BuscadorViewModel
    {
        public string KeyWords { get; set; }
    }

    public class SubirViewModel : BaseContentViewModel
    {
        public SubirViewModel(IEscuelaService escuelaService, ITipoService tipoService, INivelEducativoService nivelService, IMateriaService materiaService) : base(escuelaService, tipoService, nivelService, materiaService)
        {
        }
        public SubirViewModel()
        {

        }

        [Required]
        public new string Nombre { get; set; }
        [Required]
        new public string Descripcion { get; set; }
        public string Ruta { get; set; }
        [Required]
        new public string Profesor { get; set; }
        [Required]
        new public string Cursada { get; set; }
        new public string UsuariosId { get; set; }
        [Required]
        [Display(Name = "Escuela")]
        new public int EscuelasId { get; set; }
        [Required]
        [Display(Name = "Materia")]
        new public int MateriasId { get; set; }
        [Required]
        [Display(Name = "Nivel educativo")]
        new public int NivelesEducativosId { get; set; }
        [Required]
        [Display(Name = "Tipo de contenido")]
        new public int TiposContenidosId { get; set; }

        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public int NivNuevaEsc { get; set; }
        public string NuevaEsc { get; set; }



    }
    public class MuestraViewModel : BaseContentViewModel
    {
        public MuestraViewModel(IEscuelaService escuelaService, ITipoService tipoService, INivelEducativoService nivelService, IMateriaService materiaService) : base(escuelaService, tipoService, nivelService, materiaService)
        {

        }
        public MuestraViewModel()
        {

        }
        public int Pagina { get; set; }
        public string Title { get; set; }
        public Contenidos[] ListaAMostrar { get; set; }
        public new List<Escuelas> dropEscuela{ get; set; }

        public void Preseleccionar(string title, string nombre)
        {
            switch (title)
            {
                case "Escuelas":
                    var Escuela = escuelaService.GetEscuelasByName(nombre);
                    dropEscuela = escuelaService.GetEscuelasByNivel(Escuela.NivEduEscuela_Id);
                    EscuelasId = Escuela.Id;
                    NivelesEducativosId = Escuela.NivEduEscuela_Id;
                    break;
                case "Materias":
                    var Materia = materiaService.GetMateriaByName(nombre);
                    MateriasId = Materia.Id;
                    break;
            }
        }

        public new List<Escuelas> getEscuelas()
        {
            var Lista = new List<Escuelas>();
            Lista.Add(new Escuelas { Id = -1, Nombre = "----------" });
            return Lista;
        }

    }
    public class DetailsViewModel
    {
        public IReportesService reportService;
        public DetailsViewModel(IReportesService reportService)
        {
            this.reportService = reportService;
        }
        public DetailsViewModel()
        {

        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public List<string> Rutas { get; set; }
        public string Profesor { get; set; }
        public int Cursada { get; set; }
        public string UsuariosId { get; set; }
        public int IPop { get; set; }
        public int IDes { get; set; }
        public DateTime FechaSubida { get; set; }
        public string Badget { get; set; }
        public string ValoracionPromedio {get;set;}
        public int ValoracionUsuarioActual { get; set; }
        public virtual Usuarios Usuarios { get; set; }
        public virtual Escuelas Escuelas { get; set; }
        public virtual Materias Materias { get; set; }
        public virtual TiposContenidos TiposContenidos { get; set; }

        public Contenidos[] Recomendaciones { get; set; }
        public FormComentario FormComentario { get; set; }

    }
    public class FormComentario
    {
        public FormComentario(List<Comentarios> list, int cont)
        {
            ListaComentarios = list;
            ContenidoActualId = cont;
        }
        public FormComentario()
        {

        }
        public int Id { get; set; }
        public int ContenidoActualId { get; set; }
        public Nullable<int> ParentId { get; set; }
        [Required]
        public string Texto { get; set; }
        [Required]
        public int ContenidoId { get; set; }
        [Required]
        public string UsuarioId { get; set; }
        public List<Comentarios> ListaComentarios { get; set; }
    }
}