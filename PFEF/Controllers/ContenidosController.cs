using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PFEF.ViewModels;
using PFEF.Models;
using PFEF.Extensions;
using AutoMapper;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace PFEF.Controllers
{
    public class ContenidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private MuestraViewModel _ViewModel = new MuestraViewModel();
        // GET: Contenidos
        public ActionResult VerTodo(string Title)
        {
            _ViewModel = SwitchTitle(Title, _ViewModel);
            _ViewModel.ChargeDrops();
            Session["KeyWords"] = "";
            return View("MuestraCont",_ViewModel);
        }

        public ActionResult Descargar(int ID)
        {
            Contenidos selected = db.Contenidos.Find(ID);
            if (Request.IsAuthenticated)
            {
                ViewBag.Error = "";
                UpdateIdes(false, selected);
                string contentType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                return new FilePathResult("~/Content/Uploads/" + selected.Ruta, contentType)
                {
                    FileDownloadName = selected.Ruta,
                };
            }else
            {
                ViewBag.Error = "Necesita estar logueado para descargar documentos";
                return View("VerMas",selected);
            }
        }

        [HttpGet]
        public ActionResult Subir()
        {
            SubirViewModel SVM = new SubirViewModel();
            SVM.ChargeDrops();
            return View("SubirContenidos",SVM);
        }


        [HttpPost]
        public ActionResult Subir(SubirViewModel Cont, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    Contenidos ContMapeado = HelpersExtensions.MappearContenidos(Cont);

                    string archivo = (DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + file.FileName).ToLower();
                    archivo = archivo.Replace(" ", "");     
                    
                    ContMapeado.Ruta = archivo;
                    ContMapeado.UsuariosId = User.Identity.GetUserInfoId().Id;
                    ContMapeado.FechaSubida = DateTime.Now;
                    ContMapeado.IDes = 0;
                    ContMapeado.IPop = 0;
                                            
                    db.Contenidos.Add(ContMapeado);
                    db.SaveChanges();           

                    file.SaveAs(Server.MapPath("~/Content/Uploads/" + archivo));
                    return RedirectToAction("Index", "Home");
                }
                return View("SubirContenidos",Cont);
            }
            else
            {
                return View("SubirContenidos",Cont);
            }
        }

        [HttpGet]
        public ActionResult VerMas(int cont)
        {
            Contenidos SelectedCont = db.Contenidos.Find(cont);
            UpdateIdes(true, SelectedCont);
            if (Request.IsAuthenticated)
            {
                bool result = UpdateRecomendation(SelectedCont,User.Identity.GetUserInfoId());
            }
            var MappedCont = MapperContDetails(SelectedCont);
            ViewBag.URL = ObtenerURLArchivo(SelectedCont);
            ViewBag.Title = SelectedCont.Nombre;
            return View(MappedCont);
        }

        protected List<string> keywords = new List<string>();
        [HttpGet]
        public ActionResult Buscar(string Buscador)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = _Searcher(Buscador);
            _ViewModel.ChargeDrops();
            Session["KeyWords"] = Buscador;
            ViewBag.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
            return View("MuestraCont",_ViewModel);
        }

        [HttpPost]
        public ActionResult Buscar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            FilterParameters.ChargeDrops();
            FilterParameters.ListaAMostrar = _Searcher((string)Session["KeyWords"]);
            FilterParameters.ListaAMostrar = _Filter(FilterParameters, FilterParameters.ListaAMostrar);
            Session["Filters"] = FilterParameters;
            string Buscador = (string)Session["KeyWords"];
            ViewBag.Title = FilterParameters.Title;
            return View("MuestraCont",FilterParameters);
        }

        public ActionResult Tag(string Tag)
        {
            Session["Page"] = 0;
            _ViewModel.ListaAMostrar = _SearcherByTag(Tag);
            _ViewModel.ChargeDrops();
            ViewBag.Title = "Resultados: '" + Tag + "'";
            return View("MuestraCont", _ViewModel);
        }

        public ActionResult PasarPagina(int Pagina, string Title)
        {
            if (Pagina == -1)
            {
                Pagina = 0;
            }
            Session["Page"] = Pagina;
            _ViewModel = SwitchTitle(Title,_ViewModel);
            _ViewModel.ChargeDrops();
             return View("MuestraCont",_ViewModel);
        }

        public PartialViewResult Valoration(int Id, int star,int? Val)
        {
            DetailsViewModel MappedCont;
            using(ApplicationDbContext dbval = new ApplicationDbContext())
            {
                if(Val == null) Val = 0;
                var Cont = dbval.Contenidos.Find(Id);
                Valoraciones MiVal = new Valoraciones()
                {
                    User = new Usuarios()
                    {
                        Id = User.Identity.GetUserInfoId().Id
                    },
                    Contenido = Cont,             
                    Valoracion = star                   
                };
                dbval.Valoraciones.Add(MiVal);
                dbval.SaveChanges();

                Cont.ValoracionPromedio = dbval.Valoraciones.Where(x => x.Contenido.Id == Id).Select(x => x.Valoracion).ToList().Average();
                dbval.SaveChanges();
                MappedCont = MapperContDetails(Cont);
            }
            return PartialView("_Valoration",MappedCont);
        }
        #region Functions
        protected Contenidos[] _Searcher(string Buscador)
        {
            MuestraViewModel Buscado = new MuestraViewModel();
            if (Buscador == "")
            {
                Buscado.ListaAMostrar = db.Contenidos.OrderByDescending(x=> x.Id).ToArray();
                return Buscado.ListaAMostrar;
            }
            else
            {
                 string[] keywords = Buscador.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                this.keywords = keywords.ToList();
                int flag = 1;
                foreach (string item in keywords)
                {
                    if (flag == 1)
                    {
                        Buscado.ListaAMostrar = db.Contenidos.Where(s => s.Nombre.Contains(item) ||
                        s.Descripcion.Contains(item) || s.Profesor.Contains(item) ||
                        s.Cursada.ToString().Contains(item) || s.Usuarios.Nombre.Contains(item) ||
                        s.Escuelas.Nombre.Contains(item) ||
                        s.NivelesEducativos.Nombre.Contains(item) ||
                        s.TiposContenidos.Nombre.Contains(item) ||
                        s.Materias.Nombre.Contains(item)
                        ).ToArray();                
                        flag = 0;
                    }
                    else
                    {
                        Buscado.ListaAMostrar = Buscado.ListaAMostrar.Where(s => s.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Descripcion.ToLower().Contains(item.ToLower()) || s.Profesor.ToLower().Contains(item.ToLower()) ||
                        s.Cursada.ToString().ToLower().Contains(item.ToLower()) || s.Usuarios.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Escuelas.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.NivelesEducativos.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.TiposContenidos.Nombre.ToLower().Contains(item.ToLower()) ||
                        s.Materias.Nombre.ToLower().Contains(item.ToLower())).ToArray();
                    }
                }
                return Buscado.ListaAMostrar;
            }
        }
        protected Contenidos[] _SearcherByTag(string Buscador)
        {
            MuestraViewModel Buscado = new MuestraViewModel()
            {
                ListaAMostrar = db.Contenidos.Where(s => s.Nombre.Contains(Buscador) ||
                            s.Descripcion.Contains(Buscador) || s.Profesor.Contains(Buscador) ||
                            s.Cursada.ToString().Contains(Buscador) || s.Usuarios.Nombre.Contains(Buscador) ||
                            s.Escuelas.Nombre.Contains(Buscador) ||
                            s.NivelesEducativos.Nombre.Contains(Buscador) ||
                            s.TiposContenidos.Nombre.Contains(Buscador) ||
                            s.Materias.Nombre.Contains(Buscador)
                        ).ToArray()
            };
            return Buscado.ListaAMostrar;
        }

        protected string ObtenerURLArchivo(Contenidos model)
        {
            string URL = model.Ruta.Substring(model.Ruta.Length - 4, 4);
            if (URL == ".pdf")
            {
                URL = "https://docs.google.com/viewer?url=https://tekoteko.azurewebsites.net/Content/Uploads/" + model.Ruta + "&embedded=true";
            }
            else
            {
                URL = "https://view.officeapps.live.com/op/embed.aspx?src=https://tekoteko.azurewebsites.net/Content/Uploads/" + model.Ruta;
            }
            return URL;
        }
        protected Contenidos[] _Filter(MuestraViewModel Parameters, Contenidos[] Array)
        {
            if (Parameters.Profesor == null) Parameters.Profesor = "";
            var Lista = Array
               .Where(s => s.EscuelasId == Parameters.EscuelasId || Parameters.EscuelasId == 0)
               .Where(s => s.MateriasId == Parameters.MateriasId || Parameters.MateriasId == 0)
               .Where(s => s.TiposContenidosId == Parameters.TiposContenidosId|| Parameters.TiposContenidosId== 0)
               .Where(s => s.NivelesEducativosId == Parameters.NivelesEducativosId || Parameters.NivelesEducativosId == 0)
               .Where(s => s.Profesor.ToLower().Contains(Parameters.Profesor.ToLower()) || Parameters.Profesor == "")
               .ToArray();
            return Lista;
        }

        protected void UpdateIdes(bool DesOVis, Contenidos Cont)
        {
            if (DesOVis)
            {
                Cont.IPop++;
                db.Contenidos.Attach(Cont);
                db.Entry(Cont).Property(x => x.IPop).IsModified = true;
                db.SaveChanges();
            }
            else
            {
                Cont.IDes++;
                db.Contenidos.Attach(Cont);
                db.Entry(Cont).Property(x => x.IDes).IsModified = true;
                db.SaveChanges();
            }
        }
        protected MuestraViewModel SwitchTitle(string Title, MuestraViewModel Parameters)
        {
            switch (Title)
            {
                case "Mas recientes":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.Id).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas populares":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mas descargados":
                    _ViewModel.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                case "Mis subidas":
                    Usuarios User = (Usuarios)Session["User"];
 //                   _ViewModel.ListaAMostrar = db.Contenidos.Where(x => x.UsuariosId == User.Id).ToArray();
                    ViewBag.Title = Title;
                    return _ViewModel;
                default:
                    Parameters.ChargeDrops();
                    Parameters.ListaAMostrar = _Searcher((string)Session["KeyWords"]);

                    Parameters.ListaAMostrar = _Filter((MuestraViewModel)Session["Filters"], Parameters.ListaAMostrar);
                    ViewBag.Title = Title;
                    return Parameters;
            }
        }
        protected bool UpdateRecomendation(Contenidos cont, Usuarios User)
        {

            var FutureQuery = db.InteresesEscuelas.Where(x => x.IdUsuario.Id == User.Id && x.IdEscuela.Id == cont.Escuelas.Id).FutureFirstOrDefault();
            var FutureQuery2 = db.InteresesMaterias.Where(x => x.IdUsuario.Id == User.Id && x.IdMateria.Id == cont.Materias.Id).FutureFirstOrDefault();
            var FutureQuery3 = db.InteresesProfesores.Where(x => x.IdUsuario.Id == User.Id && x.Profesor.Contains(cont.Profesor)).FutureFirstOrDefault();

            InteresesEscuelas result = FutureQuery.Value;
            InteresesMaterias result2 = FutureQuery2.Value;
            InteresesProfesores result3 = FutureQuery3.Value;

            if (result != null)
            {
                db.InteresesEscuelas.Attach(result);
                result.Contador++;
                //db.SaveChanges();
            }
            else
            {
                InteresesEscuelas obj = new InteresesEscuelas();
                var IUser = db.Usuarios.Find(User.Id);
                obj.setEscuela(cont.Escuelas);
                obj.setUser(IUser);
                obj.Contador = 1;
                db.InteresesEscuelas.Add(obj);
                //db.SaveChanges();
            }

            if (result2 != null)
            {
                db.InteresesMaterias.Attach(result2);
                result2.Contador++;
               // db.SaveChanges();
            }
            else
            {
                InteresesMaterias obj = new InteresesMaterias();
                var IUser = db.Usuarios.Find(User.Id);
                obj.setMateria(cont.Materias);
                obj.setUser(IUser);
                obj.Contador = 1;
                db.InteresesMaterias.Add(obj);
               // db.SaveChanges();
            }
                
            if (result3 != null)
            {
                db.InteresesProfesores.Attach(result3);
                result3.Contador++;
                db.SaveChanges();
            }
            else
            {
                InteresesProfesores obj = new InteresesProfesores();
                var IUser = db.Usuarios.Find(User.Id);
                obj.Profesor = cont.Profesor;
                obj.setUser(IUser);
                obj.Contador = 1;
                db.InteresesProfesores.Add(obj);
                db.SaveChanges();
            }

            return true;
        }
        protected DetailsViewModel MapperContDetails(Contenidos cont)
        {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Contenidos, DetailsViewModel>();
                });
                IMapper mapper = config.CreateMapper();
                var ContMapeado = mapper.Map<Contenidos, DetailsViewModel>(cont);
                return ContMapeado;
        }

        #endregion
    }
}