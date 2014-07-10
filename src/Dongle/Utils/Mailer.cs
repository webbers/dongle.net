using System.Net;
using System.Net.Mail;
using Dongle.System.IO;

namespace Dongle.Utils
{
    public class Mailer
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsBodyHtml { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool SmtpEnableSsl { get; set; }
        public bool SmtpAuthenticate { get; set; }
        public int SmtpPort { get; set; }

        public void Send()
        {
            Send(Message);
        }

        public void Send(string message)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, policyErrors) => true; //ou policyErrors == SslPolicyErrors.None para validar
            var smtp = new SmtpClient(SmtpServer, SmtpPort);
            if (SmtpAuthenticate)
            {
                smtp.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            }
            smtp.EnableSsl = SmtpEnableSsl;
            smtp.Timeout = 10000;
            var mail = new MailMessage
            {
                From = new MailAddress(From),
                To = { To },
                SubjectEncoding = DongleEncoding.Default,
                Subject = Subject,
                Body = message,
                IsBodyHtml = IsBodyHtml,
            };
            smtp.Send(mail);
        }
    }
}