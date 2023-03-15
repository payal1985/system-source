using RivianAirtableIntegrationApi.Repository.Interfaces;
using System.Net;
using System.Net.Mail;

namespace RivianAirtableIntegrationApi.Repository
{
    public class EmailNotificationRepository : IEmailNotificationRepository
    {
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

        public void UpdateSendEmail(string subject, string body, string ssimanageremail)
        {
            try
            {
                var mail = new MailMessage();
                //mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));
                //mail.CC.Add(ccemail);
                //mail.Bcc.Add(Configuration.GetValue<string>("EmailConfigs:BccMail"));

                mail.To.Add(ssimanageremail);
                mail.CC.Add(Configuration.GetValue<string>("EmailConfigs:CCEmail"));
                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient.Send(mail);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void InsertSendEmail(string subject, string body, string assigneeemail)
        {
            try
            {
                var mail = new MailMessage();
                //mail.To.Add(Configuration.GetValue<string>("EmailConfigs:ToMail"));
                //mail.CC.Add(ccemail);
                //mail.Bcc.Add(Configuration.GetValue<string>("EmailConfigs:BccMail"));

                mail.To.Add(assigneeemail);
                mail.CC.Add(Configuration.GetValue<string>("EmailConfigs:BusinessCCEmail"));
                mail.CC.Add(Configuration.GetValue<string>("EmailConfigs:CCEmail"));
                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                SmtpClient.Send(mail);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public void SendErrorEmail(string subject, string body)
        {
            try
            {
                var mail = new MailMessage();
                //mail.To.Add(EmailConfigs.ToMail);

                string[] ToMaliId = Configuration.GetValue<string>("EmailConfigs:ToMail").Split(',');
                foreach (string ToEMailId in ToMaliId)
                {
                    mail.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id  
                }
                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));

                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;                
                SmtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void SendGmailEmail(string subject,string body, string ssimanageremail)
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

                mail.To.Add(ssimanageremail);
                mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), Configuration.GetValue<string>("EmailConfigs:DisplayName"));
               // mail.From = new MailAddress(Configuration.GetValue<string>("EmailConfigs:FromMail"), clientname + ' ' + Configuration.GetValue<string>("EmailConfigs:DisplayName"));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                smtp.Send(mail);
            }
        }


    }
}
