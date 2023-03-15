using SSInventory.Business.Interfaces;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Users.Authentications;
using SSInventory.Share.Models.Dto.Users.ExternalSystem;
using System.Threading.Tasks;

namespace SSInventory.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public virtual async Task<AuthenticateResponse> Authenticate(UserAuthenticationModel model, string ipAddress = "")
            => await _userRepository.Authenticate(model, ipAddress);

        public virtual async Task<AuthenticateResponse> AuthenticateFromExternal(UserAuthenticationExternalModel model, string ipAddress = "")
            => await _userRepository.AuthenticateFromExternal(model, ipAddress);

        public virtual async Task<AuthenticateResponse> RefreshToken(int userId, string token, string secret, int minutesExpired, string ipAddress = "")
            => await _userRepository.RefreshToken(userId, token, secret, minutesExpired, ipAddress);

        public virtual async Task<bool> RevokeToken(string token, string ipAddress = "")
            => await _userRepository.RevokeToken(token, ipAddress);
    }
}
