using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teko.Data.Infrastructure;
using Teko.Data.Repositories;
using Teko.Model;

namespace Teko.Service
{
    public interface IReportesService
    {
        int GetNumberReportsPerContent(int IdContenido);
        bool VerifyPrevousReports(int IdContenido, string IdUsuario);
        void AddReport(Reportes reporte);
        void SaveReport();
    }
    public class ReportesService : IReportesService
    {
        private readonly IReportesRepository reportesRepo;
        private readonly IUnitOfWork unitOfWork;

        public ReportesService(IReportesRepository reportesRepo, IUnitOfWork unitOfWork)
        {
            this.reportesRepo = reportesRepo;
            this.unitOfWork = unitOfWork;
        }

        public void AddReport(Reportes reporte)
        {
            reportesRepo.Add(reporte);
        }

        public int GetNumberReportsPerContent(int IdContenido)
        {
            return reportesRepo.GetNumberReportsPerContent(IdContenido);
        }

        public void SaveReport()
        {
            unitOfWork.Commit();
        }

        public bool VerifyPrevousReports(int IdContenido, string IdUsuario)
        {
            var Exist = reportesRepo.GetReportByContenidoAndUsuario(IdContenido, IdUsuario);
            if(Exist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
