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

            var message = new MailMessage();
            message.From = new MailAddress(emailAddress); 
            message.To.Add(email.To);
            message.Subject = email.Subject;
            message.Body = email.Body;
            
            message.IsBodyHtml = true; 


            client.Send(message);
        }
    }
}