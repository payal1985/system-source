using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.Models
{
    public class LoginModel
    {
        public int UserId { get; set; }                  
        public string UserName { get; set; }
        public int ClientId { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }

        public string ClientName { get; set; }
    }
}
