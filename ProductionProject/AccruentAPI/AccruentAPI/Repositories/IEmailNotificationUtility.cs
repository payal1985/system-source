using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccruentAPI.Repositories
{
    interface IEmailNotificationUtility
    {
        void SendEmail(string body);
    }
}
