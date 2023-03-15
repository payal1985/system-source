using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeattleInventoryApi.DBModels
{
    [Table("Users")]
    public class Login
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ClientId { get; set; }
        public string Password { get; set; }
        public bool isAdmin { get; set; }
    }
}
