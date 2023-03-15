using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvHDRequestApi.Repository.Interfaces
{
    public interface IEmailNotificationRepository
    {
        void SendEmail(string body, string name, string toemail, string clientname);
        //void SendGmailEmail(string body, string name, string ccemail, string clientname);
        //void SendGmailWarrantyEmail(string body, string name, string ccemail, string clientname);

        //void SendWarrantyEmail(string body, string name, string ccemail, string clientname);
        //void SendMaintenanceEmail(string body, string name, string ccemail, string clientname);
        //void SendCleaningEmail(string body, string name, string ccemail, string clientname);        
        void SendGenericEmail(string body, string name, string ccemail, string clientname,string reqyesttype);        
        void SendGmailEmailQueue(string body, string subject);

    }
}
