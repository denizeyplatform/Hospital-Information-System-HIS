using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IMFARepository
    {
        Task<bool> EnableMFA(string userId);
        Task<bool> VerifyMfaAsync(VerifyMfaRequest request);
    }
}
