using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryApi.Repository.Interfaces
{
    public interface IEmailNotificationRepository
    {
        void SendEmail(string body, string name, string toemail, string clientname);
        void SendGmailEmail(string body, string name, string ccemail, string clientname);
        void SendGmailWarrantyEmail(string body, string name, string ccemail, string clientname);
        void SendGmailEmailQueue(string body, string subject);

    }
}
