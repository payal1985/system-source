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
        public string Role { get; set; }
        public bool Inventory_App { get; set; }
        public string inventory_user_accept_rules_reqd { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public int? CompanyId { get; set; }
        public string StreetAddress { get; set; }
        public int? Country { get; set; }
        public int? State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }   
        
        public List<ClientModel> Clients { get; set; }

    }

    //public enum Role
    //{
    //   User="User",
    //   Admin='Admin'
    //}
}
