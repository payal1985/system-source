using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Repository
{
    public interface IEmailNotificationRepository
    {
        void SendEmail(string body, string name, string toemail,string clientname);
        void SendGmailEmail(string body, string name);
        void SendGmailEmailQueue(string body, string subject);

    }
}
