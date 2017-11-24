using System.Web;
using System.Web.Mvc;
using Teko.Models.ViewModels;
using Teko.Extensions;
using Microsoft.AspNet.Identity;
using Teko.Model;
using Teko.Service;
using System.Linq;

namespace Teko.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService usuarioService;
        private readonly IContenidoService contenidoService;
        private readonly IVisitaService visitaService;
        private readonly IEscuelaService escuelaService;


        public UsuariosController(IUsuarioService usuarioService, IEscuelaService escuelaService,IContenidoService contenidoService, IVisitaService visitaService)
        {
            this.usuarioService = usuarioService;
            this.contenidoService = contenidoService;
            this.visitaService = visitaService;
            this.escuelaService = escuelaService;
        }
        public ActionResult Index()
        {
            var MiUser = usuarioService.GetUserById(User.Identity.GetUserId());
            if (MiUser.PerfilCompleto)
            {
                PerfilViewModel model = AutoMapperGeneric<Usuarios, PerfilViewModel>.ConvertToDBEntity(MiUser);
                var lastvs = visitaService.GetVisitasByUser(MiUser.Id);
                return View("HomeUsuario",model);
            }
            else
            {
                InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(MiUser);
                MappedModel.dropEscuela = escuelaService.GetAll();
                return View("LlenarPerfil", MappedModel);
            }
        }
        [HttpGet]
        public ActionResult LlenarPerfil()
        {
            Usuarios model = usuarioService.GetUserById(User.Identity.GetUserId());
            InfoUsuarioViewModel MappedModel = AutoMapperGeneric<Usuarios, InfoUsuarioViewModel>.ConvertToDBEntity(model);
            MappedModel.dropEscuela = escuelaService.GetAll();
            return View("LlenarPerfil", MappedModel);
        }

        public PartialViewResult GetRecomendaciones()
        {
            Usuarios IUser = usuarioService.GetUserById(User.Identity.GetUserId());
            PerfilViewModel model = AutoMapperGeneric<Usuarios,PerfilViewModel>.ConvertToDBEntity(IUser);
            var lastvs = visitaService.GetVisitasByUser(IUser.Id);
            model.DictRecomendaciones = contenidoService.GetRecomendacionesByVisitas(lastvs);
            return PartialView("_Recommendation", model);
        }

        [HttpPost]
         public ActionResult LlenarPerfil(InfoUsuarioViewModel model, HttpPostedFileBase file)
        {
            var MiUser = usuarioService.GetUserById(User.Identity.GetUserId());
            file.SaveAs(Server.MapPath("~/Content/ProfilePH/" + file.FileName));
            MiUser.RutaFoto = file.FileName;
            MiUser.Descripcion = model.Descripcion;
            MiUser.InstitucionActualId = model.InstitucionActualId;
            MiUser.Nombre = model.Nombre;
            MiUser.Apellido = model.Apellido;
            MiUser.PerfilCompleto = true;
                usuarioService.UpdateUser(MiUser);
                usuarioService.SaveUser();
                return RedirectToAction("Index");      
        }

    }
}
