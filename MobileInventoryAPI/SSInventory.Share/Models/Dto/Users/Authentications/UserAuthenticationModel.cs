namespace SSInventory.Share.Models.Dto.Users.Authentications
{
    public class UserAuthenticationModel
    {
        public string LoginEmail { get; set; }
        public string LoginPW { get; set; }
        public string Secret { get; set; }
        public int MinutesExpired { get; set; }
    }
}
