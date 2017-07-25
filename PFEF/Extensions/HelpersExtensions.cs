using AutoMapper;
using PFEF.Models;
using PFEF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PFEF.Extensions
{
    public class HelpersExtensions
    {
        public static Task<int> CreateInfoUser()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Usuarios Info = new Usuarios();
                db.Usuarios.Add(Info);
                db.SaveChanges();
                var intIdt = db.Usuarios.Max(u => u.Id);
                return Task.FromResult(intIdt);
            }
        }
        public static Contenidos MappearContenidos(SubirViewModel ViewModel)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<SubirViewModel, Contenidos>();
            });
            IMapper mapper = config.CreateMapper();
            var ContMapeado = mapper.Map<SubirViewModel, Contenidos>(ViewModel);
            return ContMapeado;
        }

    }
}