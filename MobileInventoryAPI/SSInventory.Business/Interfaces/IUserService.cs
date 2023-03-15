using SSInventory.Share.Models.Dto.Users.Authentications;
using SSInventory.Share.Models.Dto.Users.ExternalSystem;
using System.Threading.Tasks;

namespace SSInventory.Business.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticateResponse> AuthenticateFromExternal(UserAuthenticationExternalModel model, string ipAddress = "");
        Task<AuthenticateResponse> Authenticate(UserAuthenticationModel model, string ipAddress = "");
        Task<AuthenticateResponse> RefreshToken(int userId, string token, string secret, int minutesExpired, string ipAddress = "");
        Task<bool> RevokeToken(string token, string ipAddress = "");
    }
}
