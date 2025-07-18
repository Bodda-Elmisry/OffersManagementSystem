using OffersManagementSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Infrastructure.Identity.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(UserAuthModel user);
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(AppIdentityUser user);
        string GenerateRefreshToken();
    }
}
