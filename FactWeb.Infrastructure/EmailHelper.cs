using FactWeb.Model.InterfaceItems;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;

namespace FactWeb.Infrastructure
{
    public static class EmailHelper
    {
        public static string factPortalEmailAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.FactPortalEmailAddress];

        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailHelper));

        public static void Send(string to, string subject, string message, bool ccFactPortal = false)
        {
            Log.Info($"Starting to send email with subject: {subject}");

            var host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailHost];
            var port = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPort];
            var userName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailUserName];
            var password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPassword];
            var fromAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailFromAddress];
            
            var msg = new MailMessage();
            msg.To.Add(new MailAddress(to));

            if (ccFactPortal)
                msg.CC.Add(new MailAddress(factPortalEmailAddress));

            msg.From = new MailAddress(fromAddress);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, password),
                Port = Convert.ToInt32(port),
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            client.Send(msg);

            Log.Info($"Finished sending email with subject: {subject}");
        }

        public static void Send(string to, string cc, string subject, string message)
        {
            Log.Info($"Starting to send email with subject: {subject}");

            var host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailHost];
            var port = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPort];
            var userName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailUserName];
            var password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPassword];
            var fromAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailFromAddress];

            var msg = new MailMessage();
            msg.To.Add(to);
            msg.CC.Add(cc);
            msg.From = new MailAddress(fromAddress);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, password),
                Port = Convert.ToInt32(port),
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            client.Send(msg);

            Log.Info($"Finished sending email with subject: {subject}");
        }

        public static void Send(List<string> to, List<string> cc, string subject, string message, Attachment attachment = null, bool ccFactPortal = false)
        {
            Log.Info($"Starting to send email with subject: {subject}");

            var host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailHost];
            var port = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPort];
            var userName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailUserName];
            var password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPassword];
            var fromAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailFromAddress];

            var msg = new MailMessage();
            foreach (var email in to)
            {
                if (!string.IsNullOrEmpty(email))
                    msg.To.Add(new MailAddress(email));
            }

            foreach (var email in cc)
            {
                if (!string.IsNullOrEmpty(email))
                    msg.CC.Add(new MailAddress(email));
            }
            if (ccFactPortal)
                msg.CC.Add(factPortalEmailAddress);
            
            msg.From = new MailAddress(fromAddress);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            if (attachment != null)
            {
                msg.Attachments.Add(attachment);
            }

            var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, password),
                Port = Convert.ToInt32(port),
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            client.Send(msg);

            Log.Info($"Finished sending email with subject: {subject}");
        }

        public static EmailMessage BuildMessage(List<string> to, List<string> cc, string subject, string message)
        {
            var host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailHost];
            var port = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPort];
            var userName = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailUserName];
            var password = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPassword];
            var fromAddress = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailFromAddress];

            var msg = new MailMessage();
            foreach (var email in to)
            {
                msg.To.Add(new MailAddress(email));
            }

            foreach (var email in cc)
            {
                msg.CC.Add(new MailAddress(email));
            }

            msg.From = new MailAddress(fromAddress);
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            var client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, password),
                Port = Convert.ToInt32(port),
                Host = host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            return new EmailMessage
            {
                SmtpClient = client,
                Message = msg
            };
        }
    }
}
