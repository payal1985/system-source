namespace RivianAirtableIntegrationApi.Repository.Interfaces
{
    public interface IEmailNotificationRepository
    {
        void UpdateSendEmail(string subject, string body, string ssimanageremail);
        void InsertSendEmail(string subject, string body, string assigneeemail);
        void SendGmailEmail(string subject, string body, string ssimanageremail);
        void SendErrorEmail(string subject, string body);
       //void SendGmailWarrantyEmail(string body, string name, string ccemail, string clientname);
        //void SendGmailEmailQueue(string body, string subject);

    }
}
