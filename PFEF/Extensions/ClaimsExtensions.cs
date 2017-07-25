using PFEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace PFEF.Extensions
{
    public static class ClaimsExtensions
    {
        public static Usuarios GetUserInfoId(this IIdentity identity)
        {
            ApplicationDbContext ClaimDb = new ApplicationDbContext();
            var claim = ((ClaimsIdentity)identity).FindFirst("IdUserInfo");
            var id = Convert.ToInt32(claim.Value);
            Usuarios User = new Usuarios();
            
            User = ClaimDb.Usuarios.Where(x => x.Id == id).FirstOrDefault();


            // Test for null to avoid issues during local testing
            return User;
     
        }
    }
}