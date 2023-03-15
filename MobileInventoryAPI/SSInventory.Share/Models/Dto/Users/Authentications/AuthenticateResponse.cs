using Newtonsoft.Json;

namespace SSInventory.Share.Models.Dto.Users.Authentications
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }
        public string TokenString { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string RolesList { get; set; }
        public string Inventory_user_accept_rules_reqd { get; set; }
        public string FristName { get; set; }
        public string PermissionLevel { get; set; }
    }
}
