using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OffersManagementSystem.Application.IServices;
using OffersManagementSystem.Application.Settings;
using OffersManagementSystem.Domain.DTOs;
using OffersManagementSystem.Infrastructure.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OffersManagementSystem.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public TokenService(UserManager<AppIdentityUser> userManager, IOptions<JwtSettings> jwtOptions)
        {
            _userManager = userManager;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(AppIdentityUser user)
        {
            // Mapping inside Infrastructure
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var userModel = new UserAuthModel
            {
                Id = user.Id,
                Email = user.Email!,
                Roles = roles,
                Claims = claims
            };

            var accessToken = await GenerateAccessTokenAsync(userModel);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userManager.UpdateAsync(user);

            return (accessToken, refreshToken);
        }

        public async Task<string> GenerateAccessTokenAsync(UserAuthModel user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        };

            claims.AddRange(user.Claims);
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
