using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Men_Accessories.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email, IConfiguration configuration)
        {
            // 1. قراءة البيانات بأمان من الإعدادات (من الـ HEAD)
            var smtpServer = configuration["EmailService:SmtpServer"];
            var port = int.Parse(configuration["EmailService:Port"]);
            var emailAddress = configuration["EmailService:Email"];
            var password = configuration["EmailService:Password"];

            var client = new SmtpClient(smtpServer, port);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(emailAddress, password);

            // 2. تجهيز رسالة الإيميل (من الـ Incoming)
            var message = new MailMessage();
            message.From = new MailAddress(emailAddress); // استخدمنا الإيميل اللي جي من الـ Config
            message.To.Add(email.To);
            message.Subject = email.Subject;
            message.Body = email.Body;
            
            // IMPORTANT
            message.IsBodyHtml = true; 

            // 3. الإرسال
            client.Send(message);
        }
    }
}