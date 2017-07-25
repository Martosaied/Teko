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

namespace PFEF.Controllers
{
    public class ContenidosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MuestraViewModel FVM = new MuestraViewModel();
        // GET: Contenidos
        public ActionResult VerTodo(string Title)
        {
            FVM = SwitchTitle(Title,FVM);
            FVM = CargarDropsFVM(FVM);
            Session["KeyWords"] = "";
            return View("MuestraCont",FVM);
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
            SVM = CargarDropsSVM(SVM);
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
                bool result = UpdateCustomDict(SelectedCont,User.Identity.GetUserInfoId());
            }
            //vamos bien falta el sistema de si existe sumralo sino crearlo en el diccionario
            ViewBag.URL = ObtenerURLArchivo(SelectedCont);
            ViewBag.Title = SelectedCont.Nombre;
            return View(SelectedCont);//porfavor
        }

        protected List<string> keywords = new List<string>();
        [HttpGet]
        public ActionResult Buscar(string Buscador)
        {
            Session["Page"] = 0;
            FVM.ListaAMostrar = _Searcher(Buscador);
            FVM = CargarDropsFVM(FVM);
            Session["KeyWords"] = Buscador;
            ViewBag.Title = "Resultados: '" + Buscador.Replace(" ", "' '") + "'";
            return View("MuestraCont",FVM);
        }

        [HttpPost]
        public ActionResult Buscar(MuestraViewModel FilterParameters)
        {
            Session["Page"] = 0;
            FilterParameters = CargarDropsFVM(FilterParameters);
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
            FVM.ListaAMostrar = _SearcherByTag(Tag);
            FVM = CargarDropsFVM(FVM);
            ViewBag.Title = "Resultados: '" + Tag + "'";
            return View("MuestraCont", FVM);
        }

        public ActionResult PasarPagina(int Pagina, string Title)
        {
            if (Pagina == -1)
            {
                Pagina = 0;
            }
            Session["Page"] = Pagina;
            FVM = SwitchTitle(Title,FVM);
            FVM = CargarDropsFVM(FVM);
             return View("MuestraCont",FVM);
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
               .Where(s => s.EscuelasId == Parameters.IdEscuela || Parameters.IdEscuela == 0)
               .Where(s => s.MateriasId == Parameters.IdMateria || Parameters.IdMateria == 0)
               .Where(s => s.TiposContenidosId == Parameters.IdTipoContenido || Parameters.IdTipoContenido == 0)
               .Where(s => s.NivelesEducativosId == Parameters.IdNivelEducativo || Parameters.IdNivelEducativo == 0)
               .Where(s => s.Profesor.ToLower().Contains(Parameters.Profesor.ToLower()) || Parameters.Profesor == "")
               .ToArray();
            return Lista;
        }
        protected MuestraViewModel CargarDropsFVM(MuestraViewModel FVM)
        {
            FVM.dropEscuela = db.Escuelas.ToList();
            FVM.dropMateria = db.Materias.ToList();
            FVM.dropTipoContenido = db.TiposContenidos.ToList();
            FVM.dropNivelEducativo = db.NivelesEducativos.ToList();
            return FVM;
        }
        protected SubirViewModel CargarDropsSVM(SubirViewModel SVM)
        {
            SVM.dropEscuela = db.Escuelas.ToList();
            SVM.dropMateria = db.Materias.ToList();
            SVM.dropTipoContenido = db.TiposContenidos.ToList();
            SVM.dropNivelEducativo = db.NivelesEducativos.ToList();

            return SVM;
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
                    FVM.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.Id).ToArray();
                    ViewBag.Title = Title;
                    return FVM;
                case "Mas populares":
                    FVM.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return FVM;
                case "Mas descargados":
                    FVM.ListaAMostrar = db.Contenidos.OrderByDescending(x => x.IPop).ToArray();
                    ViewBag.Title = Title;
                    return FVM;
                case "Mis subidas":
                    Usuarios User = (Usuarios)Session["User"];
 //                   FVM.ListaAMostrar = db.Contenidos.Where(x => x.UsuariosId == User.Id).ToArray();
                    ViewBag.Title = Title;
                    return FVM;
                default:
                    Parameters = CargarDropsFVM(Parameters);
                    Parameters.ListaAMostrar = _Searcher((string)Session["KeyWords"]);

                    Parameters.ListaAMostrar = _Filter((MuestraViewModel)Session["Filters"], Parameters.ListaAMostrar);
                    ViewBag.Title = Title;
                    return Parameters;
            }
        }
        protected bool UpdateCustomDict(Contenidos cont, Usuarios User)
        {
            var result = db.InteresesEscuelas.Where(x => x.IdUsuario.Id == User.Id && x.IdEscuela.Id == cont.Escuelas.Id).FirstOrDefault();    
            if(result != null)
            {
                db.InteresesEscuelas.Attach(result);
                result.Contador++;
                db.SaveChanges();
            }
            else
            {
                InteresesEscuelas obj = new InteresesEscuelas();
                var IUser = db.Usuarios.Find(User.Id);
                obj.setEscuela(cont.Escuelas);
                obj.setUser(IUser);
                obj.Contador = 1;
                db.InteresesEscuelas.Add(obj);
                db.SaveChanges();
            }

            var result2 = db.InteresesMaterias.Where(x => x.IdUsuario.Id == User.Id && x.IdMateria.Id == cont.Materias.Id).FirstOrDefault();
            if (result2 != null)
            {
                db.InteresesMaterias.Attach(result2);
                result2.Contador++;
                db.SaveChanges();
            }
            else
            {
                InteresesMaterias obj = new InteresesMaterias();
                var IUser = db.Usuarios.Find(User.Id);
                obj.setMateria(cont.Materias);
                obj.setUser(IUser);
                obj.Contador = 1;
                db.InteresesMaterias.Add(obj);
                db.SaveChanges();
            }
                
            var result3 = db.InteresesProfesores.Where(x => x.IdUsuario.Id == User.Id && x.Profesor.Contains(cont.Profesor)).FirstOrDefault();
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
        #endregion
    }
}