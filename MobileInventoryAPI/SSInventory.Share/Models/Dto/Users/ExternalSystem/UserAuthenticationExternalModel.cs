namespace SSInventory.Share.Models.Dto.Users.ExternalSystem
{
    public class UserAuthenticationExternalModel
    {
        public string Secret { get; set; }
        public int MinutesExpired { get; set; }
        public UserLoginResponseModel User { get; set; }
    }
}
