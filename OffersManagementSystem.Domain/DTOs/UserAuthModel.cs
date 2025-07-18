using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OffersManagementSystem.Domain.DTOs
{
    public class UserAuthModel
    {
        public string Id { get; set; } = default!;
        public string Usernaem { get; set; } = default!;
        public IList<string> Roles { get; set; } = new List<string>();
        public IList<Claim> Claims { get; set; } = new List<Claim>();
    }
}
