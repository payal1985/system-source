using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoginInventoryApi.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public int ClientId { get; set; }
        //public string ClientName { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool isAdmin { get; set; }
        public string role { get; set; }

    }

    //public enum Role
    //{
    //   User="User",
    //   Admin='Admin'
    //}
}
