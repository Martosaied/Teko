using System.Net;
using System.Net.Mail;
using Teko.Data.Infrastructure;
using Teko.Data.Repositories;
using Teko.Model;

namespace Teko.Service
{
    public interface IMailService
    {
        bool SendReportEmail(dynamic reportInfo);
    }
    public class MailService : IMailService
    {
        private readonly IUsuarioRepository userRepo;
        private readonly IUnitOfWork unitOfWork;

        public MailService(IUsuarioRepository userRepo, IUnitOfWork unitOfWork)
        {
            this.userRepo = userRepo;
            this.unitOfWork = unitOfWork;
        }


        public bool SendReportEmail(dynamic reportInfo)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("tteko357@gmail.com", "Teko2017"),
                EnableSsl = true
            };
            client.Send(reportInfo.reportedUser.Email, "tteko357@gmail.com", reportInfo.reportedUser.UserName + " - " + reportInfo.reportedContenido.Nombre + " - "+ reportInfo.Titulo, reportInfo.Texto + " - " + reportInfo.reportedUser.Email + " - " + reportInfo.URL);
            return true;
        }
    }
}
