using Microsoft.Extensions.Configuration;
using SSInventory.Web.Models.Email;
using System.Net.Mail;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Net;

namespace SSInventory.Web.Services.Email
{
    public class NetMailSender
    {
        private readonly IConfiguration _config;
        private readonly MailSettings _mailSettings;
        public NetMailSender(IConfiguration config)
        {
            _config = config;

            _mailSettings = GetMailSettings(_config);
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(mailRequest.ToEmail);
                if (mailRequest.CCs?.Count > 0)
                {
                    foreach (var cc in mailRequest.CCs)
                    {
                        mail.CC.Add(cc);
                    }
                }
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            mail.Attachments.Add(new Attachment(file.FileName));
                        }
                    }
                }
                else if (mailRequest.FileAttachments?.Count > 0)
                {
                    foreach (var file in mailRequest.FileAttachments)
                    {
                        if (!string.IsNullOrWhiteSpace(file.FileName) && !string.IsNullOrWhiteSpace(file.FileType) && file.File.Length > 0)
                        {
                            var memStream = new MemoryStream(file.File)
                            {
                                Position = 0
                            };
                            var reportAttachment = new Attachment(memStream, file.FileType);
                            reportAttachment.ContentDisposition.FileName = file.FileName;

                            mail.Attachments.Add(reportAttachment);
                        }
                    }
                }

                mail.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
                mail.Subject = mailRequest.Subject;
                mail.Body = mailRequest.Body;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                SmtpClient client = new SmtpClient
                {
                    Host = _mailSettings.Host,
                    Port = _mailSettings.Port,
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception ex)
            {
                
            }
        }

        private MailSettings GetMailSettings(IConfiguration configuration) => new MailSettings
        {
            Mail = configuration.GetValue<string>("EmailConfigure:Settings:Mail"),
            Password = configuration.GetValue<string>("EmailConfigure:Settings:Password"),
            DisplayName = configuration.GetValue<string>("EmailConfigure:Settings:DisplayName"),
            Host = configuration.GetValue<string>("EmailConfigure:Settings:Host"),
            Port = configuration.GetValue<int>("EmailConfigure:Settings:Port"),
        };
    }
}
