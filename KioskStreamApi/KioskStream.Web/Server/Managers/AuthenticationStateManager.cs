using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using KioskStream.Core.Configurations;
using KioskStream.Core.Enums;
using KioskStream.Data;
using KioskStream.Data.Models;
using KioskStream.Models;
using KioskStream.Web.Common.DataTransferObjects.Account;
using KioskStream.Web.Server.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KioskStream.Web.Server.Managers
{
    public class AuthenticationStateManager : IAuthenticationStateManager
    {
        private readonly ApplicationConfiguration _applicationConfiguration;

        private readonly IRepository<User> _userRepository;
        
        private readonly IRepository<RefreshToken> _refreshTokenRepository;

        public AuthenticationStateManager(
            IOptions<ApplicationConfiguration> applicationConfiguration,
            IRepository<User> userRepository,
            IRepository<RefreshToken> refreshTokenRepository)
        {
            _applicationConfiguration = applicationConfiguration.Value;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(nameof(EAuthorizationClaims.FullName), $"{user.FirstName} {user.LastName}")
            };
                
            return claims;
        }

        public async Task<AuthenticationResult> Authenticate(int userId, int? refreshTokenId, IEnumerable<Claim> claims)
        {
            DateTime accessTokenExpiry = DateTime.UtcNow.AddMinutes(_applicationConfiguration.Authentication.ExpiryDurationMinutes);
            DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(_applicationConfiguration.Authentication.ExpiryDurationMinutes);
            string accessToken = GenerateAccessToken(claims, accessTokenExpiry);
            string refreshToken;
            if (refreshTokenId == null)
            {
                refreshToken = GenerateRefreshToken();
                await SaveRefreshToken(userId, refreshTokenExpiry, refreshToken);
            }
            else
            {
                refreshToken = await SetRefreshTokenExpiry((int)refreshTokenId, refreshTokenExpiry);
            }
            return new AuthenticationResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims, DateTime accessTokenExpiry)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationConfiguration.Authentication.SecretKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _applicationConfiguration.Authentication.Issuer,
                audience: _applicationConfiguration.Authentication.Audience,
                claims: claims,
                expires: accessTokenExpiry,
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task SaveRefreshToken(int userId, DateTime refreshTokenExpiry, string refreshToken)
        {
            User user = await _userRepository.Get(u => u.Id == userId).FirstOrDefaultAsync();
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                Expires = refreshTokenExpiry
            });
            await _userRepository.SaveChangesAsync();
        }

        private async Task<string> SetRefreshTokenExpiry(int refreshTokenId, DateTime refreshTokenExpiry)
        {
            RefreshToken refreshToken = await _refreshTokenRepository.Get(r => r.Id == refreshTokenId).FirstOrDefaultAsync();
            refreshToken.Expires = refreshTokenExpiry;
            await _refreshTokenRepository.SaveChangesAsync();
            return refreshToken.Token;
        }

        //private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        //{
        //    TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateAudience = true,
        //        ValidAudience = _jwtParameters.Audience,
        //        ValidIssuer = _jwtParameters.Issuer,
        //        ValidateIssuer = true,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtParameters.SecretKey)),
        //        ValidateLifetime = false
        //    };
        //    ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        //    JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        //    if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        //        throw new SecurityTokenException("Invalid token");
        //    return principal;
        //}
    }
}
