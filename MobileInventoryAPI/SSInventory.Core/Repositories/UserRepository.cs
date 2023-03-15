using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SSInventory.Core.Models;
using SSInventory.Core.Repositories.Interfaces;
using SSInventory.Share.Models.Dto.Users.Authentications;
using SSInventory.Share.Models.Dto.Users.ExternalSystem;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SSInventory.Core.Repositories
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        private readonly IMapper _mapper;
        public UserRepository(SSInventoryDbContext dbContext, IMapper mapper)
            : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> AuthenticateFromExternal(UserAuthenticationExternalModel model, string ipAddress = "")
        {
            // If authentication successful, generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(model.User.UserId, model.Secret, model.MinutesExpired);
            var refreshToken = GenerateRefreshToken(model.User.UserId, model.MinutesExpired, ipAddress);
            refreshToken.IsActive = refreshToken.Revoked == null && !refreshToken.IsExpired;
            refreshToken.IsExpired = DateTime.UtcNow >= refreshToken.Expires;

            // save refresh token
            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthenticateResponse
            {
                UserId = model.User.UserId,
                UserName = model.User.UserName,
                //UserEmail = model.User.Email,
                RolesList = model.User.UserType == "Admin" ? "Admin" : "User",
                TokenString = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticateResponse> Authenticate(UserAuthenticationModel model, string ipAddress = "")
        {
            var user = await GetAll().FirstOrDefaultAsync(x => x.UserName == model.LoginEmail && x.Password == model.LoginPW);

            // return null if user not found
            if (user == null) return null;

            var listRoles = await GetRoleNames(user.UserId);

            // If authentication successful, generate jwt and refresh tokens
            var jwtToken = GenerateJwtToken(user, model.Secret, model.MinutesExpired);
            var refreshToken = GenerateRefreshToken(user.UserId, model.MinutesExpired, ipAddress);
            refreshToken.IsActive = refreshToken.Revoked == null && !refreshToken.IsExpired;
            refreshToken.IsExpired = DateTime.UtcNow >= refreshToken.Expires;
            refreshToken.UserId = user.UserId;

            await _dbContext.RefreshTokens.AddAsync(refreshToken);
            await _dbContext.SaveChangesAsync();

            return new AuthenticateResponse
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserName,
                RolesList = string.Join(", ", listRoles),
                TokenString = jwtToken,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticateResponse> RefreshToken(int userId, string token, string secret, int minutesExpired, string ipAddress = "")
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token && x.IsActive.HasValue && x.IsActive.Value && !x.IsExpired);

            // return null if token is no longer active
            if (refreshToken is null || refreshToken?.IsActive == false) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(userId, minutesExpired, ipAddress);
            refreshToken.Revoked = DateTime.Now;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            refreshToken.IsActive = refreshToken.Revoked == null && !refreshToken.IsExpired;
            refreshToken.IsExpired = DateTime.UtcNow >= refreshToken.Expires;

            await _dbContext.RefreshTokens.AddAsync(newRefreshToken);
            _dbContext.RefreshTokens.Update(refreshToken);
            await _dbContext.SaveChangesAsync();

            // generate new jwt
            var jwtToken = GenerateJwtToken(userId, secret, minutesExpired);

            return new AuthenticateResponse
            {
                TokenString = jwtToken,
                RefreshToken = newRefreshToken.Token,
                UserId = userId
            };
        }

        public async Task<bool> RevokeToken(string token, string ipAddress = "")
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstAsync(x => x.Token == token);

            // return false if token is not active
            if (refreshToken?.IsActive != true) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.IsActive = false;
            refreshToken.IsExpired = true;
            _dbContext.RefreshTokens.Update(refreshToken);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        #region Private Methods
        private string GenerateJwtToken(Users user, string secret, int minutesExpired)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutesExpired),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(int userId, string secret, int minutesExpired)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(minutesExpired),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokens GenerateRefreshToken(int userId, int minutesExpired, string ipAddress = "")
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokens
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddMinutes(minutesExpired),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserId = userId,
                IsActive = true,
                IsExpired = false
            };
        }

        private async Task<List<string>> GetRoleNames(int userId)
        {
            return await (from userRoles in _dbContext.UserRoles
                          join roles in _dbContext.Roles on userRoles.RoleId equals roles.RoleId
                          where userRoles.UserId == userId
                          select roles.RoleName).Distinct().ToListAsync();
        }

        #endregion
    }
}
