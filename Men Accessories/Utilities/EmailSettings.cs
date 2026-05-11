using System.Net;
using System.Net.Mail;

namespace Men_Accessories.Utilities
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("dinaalaraby9503@gmail.com","rbvbeozaihejygep");
            client.Send("dinaalaraby9503@gmail.com",email.To,email.Subject,email.Body);
        }
    }
}
