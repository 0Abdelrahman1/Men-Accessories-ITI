using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Men_Accessories.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email, IConfiguration configuration)
        {
            var smtpServer = configuration["EmailService:SmtpServer"];
            var port = int.Parse(configuration["EmailService:Port"]);
            var emailAddress = configuration["EmailService:Email"];
            var password = configuration["EmailService:Password"];

            var client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(emailAddress, password);
            client.Send(emailAddress, email.To, email.Subject, email.Body);
        }
    }
}
