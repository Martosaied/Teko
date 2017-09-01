using AutoMapper;
using Microsoft.AspNet.Identity;
using PFEF.Controllers;
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
        public static ApplicationDbContext db;
        
        public static Usuarios ObtenerUser(string Id)
        {
            db = new ApplicationDbContext();
                var AU = db.Users.Where(x => x.Id == Id).FirstOrDefault();
            if(AU == null) { return null; }
                Usuarios MiUser = db.Usuarios.Where(x => x.Id == AU.IdUserInfo).FirstOrDefault();
                return MiUser; 
        }
        public static Task<int> CreateInfoUser()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Usuarios Info = new Usuarios();
                Info.PerfilCompleto = false;
                db.Usuarios.Add(Info);
                db.SaveChanges();
                var intIdt = db.Usuarios.Max(u => u.Id);
                return Task.FromResult(intIdt);
            }
        }
        public static Task<int> CreateInfoUser(string idU)
        {

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Usuarios Info   = new Usuarios();
                db.Usuarios.Add(Info);

                var result = db.Users.Find(idU);
                result.IdUserInfo = db.Usuarios.Max(u => u.Id);

                db.SaveChanges();
                var intIdt = 1;
                return Task.FromResult(intIdt);
            }
        }
        public static string BudgetFileSelect(int IdCont)
        {
            string ADevolver = "fa fa-file-o";
            var List = db.Archivos.Where(x => x.IdContenido.Id == IdCont).FirstOrDefault();
            if(List == null)
            {
                return ADevolver;
            }
            string URL = List.Ruta.Substring(List.Ruta.Length - 4, 4);
            switch (URL)
            {
                case "docx":
                case ".doc":
                    ADevolver = "fa fa-file-word-o";
                    break;
                case ".xls":
                case "xlsx":
                    ADevolver = "fa fa-file-excel-o";
                    break;
                case ".ppt":
                case "pptx":
                    ADevolver = "fa fa-file-powerpoint-o";
                    break;
                case ".pdf":
                    ADevolver = "fa fa-file-pdf-o";
                    break;
                default:
                    break;
            }
            return ADevolver;
        }

    }
}