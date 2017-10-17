using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Data.Infrastructure;
using Teko.Model;

namespace Teko.Data.Repositories
{
    public class ReportesRepository : RepositoryBase<Reportes>, IReportesRepository
    {
        public ReportesRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public int GetNumberReportsPerContent(int IdContenido)
        {
            int CantReportes = DbContext.Reportes.Where(x => x.IdContenido == IdContenido).Count();
            return CantReportes;
        }

        public Reportes GetReportByContenidoAndUsuario(int idContenido, string idUsuario)
        {
            var Contenido = DbContext.Reportes.Where(x => x.IdContenido == idContenido && x.IdUsuario == idUsuario).FirstOrDefault();
            return Contenido;
        }
    }
    public interface IReportesRepository : IRepository<Reportes>
    {
        int GetNumberReportsPerContent(int IdContenido);
        Reportes GetReportByContenidoAndUsuario(int idContenido, string idUsuario);
    }

}
