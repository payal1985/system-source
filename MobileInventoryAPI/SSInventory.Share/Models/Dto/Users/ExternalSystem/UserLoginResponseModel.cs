namespace SSInventory.Share.Models.Dto.Users.ExternalSystem
{
    public class UserLoginResponseModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public bool isAdmin { get; set; }
        public string role { get; set; }
        public string Inventory_user_accept_rules_reqd { get; set; }
        public string PermissionLevel { get; set; }
    }
}
