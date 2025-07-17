using OffersManagementSystem.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Application.IServices
{
    public interface ITokenService
    {
        Task<string> GenerateAccessTokenAsync(UserAuthModel user);
        string GenerateRefreshToken();
    }
}
