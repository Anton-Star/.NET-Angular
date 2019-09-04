using FactWeb.BusinessLayer;
using FactWeb.Infrastructure;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

namespace FactWeb.BusinessFacade
{
    public class BugFacade
    {
        private readonly Container container;

        public BugFacade(Container container)
        {
            this.container = container;
        }

        public void AddBug(string text, string version, DateTime? releaseDate, string url, string createdBy, string systemUrl, Guid? appUniqueId)
        {
            var manager = this.container.GetInstance<BugManager>();
            var appSettingManager = this.container.GetInstance<ApplicationSettingManager>();

            //manager.AddBug(text, version, releaseDate, url, createdBy);

            var appSetting = appSettingManager.GetByName(Constants.ApplicationSettings.AutoCcEmailAddress);

            var coordinatorName = "<img src=\"http://fact5mdev.azurewebsites.net/content/images/fact-head.png\" height=\"60\" />";
            var coordinatorTitle = "<b>Foundation for the Accreditation of Cellular Therapy</b>";
            var coordinatorPhone = "University of Nebraska Medical Center";
            var coordinatorEmail =
                "986065 Nebraska Medical Center  |  Omaha, NE 68198-6065<br />402.559.1950  |  fax 402.559.1951";

            var ccs = new List<string>
            {
                createdBy
            };

            if (appUniqueId.HasValue)
            {
                var appManager = this.container.GetInstance<ApplicationManager>();
                var app = appManager.GetByUniqueId(appUniqueId.Value, false, false);

                if (app != null && (app.Coordinator != null || app.ComplianceApplication?.Coordinator != null))
                {
                    var coordinator = app.Coordinator ?? app.ComplianceApplication.Coordinator;

                    var creds = coordinator.UserCredentials != null
                        ? ", " + string.Join(", ", coordinator.UserCredentials.Select(x => x.Credential.Name))
                        : "";
                   
                    coordinatorName = $"{coordinator.FirstName} {coordinator.LastName}{creds}";
                    coordinatorTitle = coordinator.Title;
                    coordinatorPhone = coordinator.PreferredPhoneNumber;
                    coordinatorEmail = coordinator.EmailAddress;
                    ccs.Add(coordinator.EmailAddress);
                }
            }

            var page = systemUrl + "app/email.templates/bug.html";
            var title = "FACT Accreditation Portal Issue";

            var html = WebHelper.GetHtml(page);
            html = html.Replace("{Url}", url);
            html = html.Replace("{Time}", DateTime.Now.ToString("G"));
            html = html.Replace("{User}", createdBy);
            html = html.Replace("{CoordinatorName}", coordinatorName);
            html = html.Replace("{CoordinatorTitle}", coordinatorTitle);
            html = html.Replace("{CoordinatorPhone}", coordinatorPhone);
            html = html.Replace("{CoordinatorEmail}", coordinatorEmail);

            if (text.Contains("base64,"))
            {
                var txt = text.Substring(text.IndexOf("base64,", StringComparison.Ordinal) + 7);
                txt = txt.Substring(0, txt.IndexOf("\"", StringComparison.Ordinal));

                var bytes = Convert.FromBase64String(txt);

                using (var ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;

                    var attachment = new Attachment(ms, "image.png", "image/png");
                    attachment.ContentDisposition.Inline = true;

                    var inline = new LinkedResource(ms) {ContentId = Guid.NewGuid().ToString()};

                    text = text.Replace("data:image/png;base64," + txt, "cid:" + inline.ContentId);

                    html = html.Replace("{Comments}", text);

                    var avHtml = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
                    avHtml.LinkedResources.Add(inline);

                    var from =ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailFromAddress];

                    var message = new MailMessage(from, appSetting.Value) {IsBodyHtml = true, Subject = title};
                    message.AlternateViews.Add(avHtml);

                    var client = new SmtpClient
                    {
                        UseDefaultCredentials = false,
                        Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailUserName], ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPassword]),
                        Port = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailPort]),
                        Host = ConfigurationManager.AppSettings[Constants.ConfigurationConstants.EmailHost],
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true
                    };

                    client.Send(message);

                }
                    
            }
            else
            {
                html = html.Replace("{Comments}", text);

                EmailHelper.Send(new List<string> { appSetting.Value }, ccs, title, html);
            }
        }
    }
}
