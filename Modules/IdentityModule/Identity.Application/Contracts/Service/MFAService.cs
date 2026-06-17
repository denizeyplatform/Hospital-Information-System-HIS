using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Service
{
    public class MFAService : IMFAService
    {
        public readonly IMFARepository _mfaRepository;
        public MFAService(IMFARepository mfaRepository)
        {
            _mfaRepository = mfaRepository;
        }
        public async Task<bool> EnableMFAAsync(string userId)
        {
            return await _mfaRepository.EnableMFA(userId);
        }

        public async Task<UserResponseDTO?> VerifyMfaAsync(VerifyMfaRequest request)
        {
            // raise the mfa verification event here
            return await _mfaRepository.VerifyMfaAsync(request);
        }
    }
}
