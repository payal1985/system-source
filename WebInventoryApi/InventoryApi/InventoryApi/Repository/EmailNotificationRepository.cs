using InventoryApi.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace InventoryApi.Repository
{
    public class EmailNotificationRepository : IEmailNotificationRepository
    {
        //private EmailConfigs EmailConfigs { get; set; }
        public IConfiguration Configuration { get; }

        private SmtpClient SmtpClient { get; set; }

        public EmailNotificationRepository(IConfiguration configuration)
        {
            // EmailConfigs = emailConfigs;
            Configuration = configuration;

            SmtpClient = new SmtpClient
            {
                //username = _config.GetValue<string>("GoCanvasAccessCredential:UserName");
                Host = Configuration.GetValue<string>("EmailConfigs:Host"),
                Port = Configuration.GetValue<int>("EmailConfigs:Port"),
                UseDefaultCredentials = true,
                EnableSsl = false,
                Timeout = 30 * 1000,
               
        };
        }
        public void SendEmail(string body,string name,string ccemail, string clientname)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));
                mail.CC.Add(ccemail);
                mail.Bcc.Add(Configuration.GetValue<string>("EmailConfigs:BccMail"));

                //mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), clientname + ' ' + Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                //mail.Subject = Configuration.GetValue<string>("EmailConfigs:Subject")+ ' '+ name;
                mail.Subject = $"{clientname} {Configuration.GetValue<string>("EmailConfigs:Subject")} {name}";
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void SendGmailEmail(string body, string name, string ccemail, string clientname)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("payal.sky2007@gmail.com", "sriSai@@2021");
                // send the email

                var mail = new MailMessage();

                mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));

                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), clientname + ' ' + Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                //mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                //mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"));
                //mail.Subject = Configuration.GetValue<string>("EmailConfigs:Subject") + ' ' + name;
                mail.Subject = $"{clientname} {Configuration.GetValue<string>("EmailConfigs:Subject")} {name}";
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
        }

        public void SendGmailWarrantyEmail(string body, string name, string ccemail, string clientname)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("payal.sky2007@gmail.com", "sriSai@@2021");
                // send the email

                var mail = new MailMessage();

                mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));

                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), clientname + ' ' + "Inventory Warranty Notification");
                //mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                //mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"));
                //mail.Subject = Configuration.GetValue<string>("EmailConfigs:Subject") + ' ' + name;
                mail.Subject = $"{clientname} Inventory Warranty for  {name}";
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
        }

        public void SendGmailEmailQueue(string body,string subject)
        {
            using (SmtpClient smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new NetworkCredential("payal.sky2007@gmail.com", "sriSai@@2021");
                // send the email

                var mail = new MailMessage();

                mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));

                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                smtp.Send(mail);
            }
        }
    }
}
