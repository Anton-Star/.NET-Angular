using System.Net.Mail;

namespace FactWeb.Model.InterfaceItems
{
    public class EmailMessage
    {
        public SmtpClient SmtpClient { get; set; }
        public MailMessage Message { get; set; }
    }
}
