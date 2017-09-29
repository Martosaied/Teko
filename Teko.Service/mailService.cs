using Teko.Data.Infrastructure;
using Teko.Data.Repositories;

namespace Teko.Service
{
    public interface IMailService
    {

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

        /*public bool SendRegistracionMail(string EmailUser)
        {
            try
            { 
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(EmailUser);
                mailMessage.From = new MailAddress("teko@tekopeola.com");
                mailMessage.Subject = "ASP.NET e-mail test";
                mailMessage.Body = "Hello world,\n\nThis is an ASP.NET test e-mail!";
                SmtpClient smtpClient = new SmtpClient("smtp.your-isp.com");
                smtpClient.Send(mailMessage);
                Response.Write("E-mail sent!");
            }
            catch (Exception ex)
            {
                Response.Write("Could not send the e-mail - error: " + ex.Message);
            }
        }*/
    }
}
