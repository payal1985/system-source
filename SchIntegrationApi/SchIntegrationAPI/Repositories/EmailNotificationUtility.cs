using SchIntegrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Web;

namespace SchIntegrationAPI.Repositories
{
    public class EmailNotificationUtility : IEmailNotificationUtility
    {
        private EmailConfigs EmailConfigs { get; set; }
        private SmtpClient SmtpClient { get; set; }

        public EmailNotificationUtility(EmailConfigs emailConfigs)
        {
            EmailConfigs = emailConfigs;

            //TestSocketConention();

            SmtpClient = new SmtpClient
            {
                Host = EmailConfigs.Host,
                Port = EmailConfigs.Port,
                UseDefaultCredentials = true,
                EnableSsl = false,
                Timeout = 30 * 1000,
            };
        }

        private void TestSocketConention()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var result = socket.BeginConnect(EmailConfigs.Host, EmailConfigs.Port, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(10 * 1000, true);
            if (!success)
            {
                socket.Close();
                throw new Exception("Failed to connect to email service.");
            }
        }

        public void SendEmail(string body)
        {
            try
            {
                var mail = new MailMessage();
                //mail.To.Add(EmailConfigs.ToMail);

                string[] ToMaliId = EmailConfigs.ToMail.Split(',');
                foreach (string ToEMailId in ToMaliId)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }
                mail.From = new MailAddress(EmailConfigs.FromMail, EmailConfigs.DisplayName);
                mail.Subject = EmailConfigs.Subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                SmtpClient.Send(mail);
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public void SendEmailToBusiness(string body,string subject)
        {
            try
            {
                var mail = new MailMessage();
                //mail.To.Add(EmailConfigs.ToMail);

                string[] ToMaliId = EmailConfigs.ToMailBusiness.Split(',');
                foreach (string ToEMailId in ToMaliId)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }
                mail.From = new MailAddress(EmailConfigs.FromMail, subject);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SendGmailEmail(string body)
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
                
                string[] ToMaliId = EmailConfigs.ToMail.Split(',');
                foreach (string ToEMailId in ToMaliId)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }
               // mail.To.Add("");
                mail.From = new MailAddress(EmailConfigs.FromMail, EmailConfigs.DisplayName);
                mail.Subject = EmailConfigs.Subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                smtp.Send(mail);
            }
        }
    }
}