using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SSInventory.Web.Models.Email;
using System.IO;
using System.Threading.Tasks;

namespace SSInventory.Web.Services.Email
{
    public class MailService
    {
        private readonly IConfiguration _config;
        private readonly MailSettings _mailSettings;
        public MailService(IConfiguration config)
        {
            _config = config;

            _mailSettings = GetMailSettings(_config);
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject
            };
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            if (mailRequest.CCs?.Count > 0)
            {
                foreach (var cc in mailRequest.CCs)
                {
                    email.Cc.Add(InternetAddress.Parse(cc));
                }
            }

            var builder = new BodyBuilder();
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
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            else if(mailRequest.FileAttachments?.Count > 0)
            {
                foreach (var file in mailRequest.FileAttachments)
                {
                    if(!string.IsNullOrWhiteSpace(file.FileName) && !string.IsNullOrWhiteSpace(file.FileType) && file.File.Length > 0)
                    {
                        builder.Attachments.Add(file.FileName, file.File, ContentType.Parse(file.FileType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
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
