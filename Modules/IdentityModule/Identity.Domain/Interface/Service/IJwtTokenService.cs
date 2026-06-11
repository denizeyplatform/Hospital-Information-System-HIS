using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interface.Service
{
    public interface IJwtTokenService
    {
        Task<string> GenerateToken(string userId);
        string GenerateRefreshToken();
    }
}
