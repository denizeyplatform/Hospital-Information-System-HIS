using Identity.Application.Contracts.Interface;
using Identity.Application.Contracts.Interface.Repository;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Interface.Service;

namespace Identity.Application.Contracts.Service
{
    public class AuthService : IAuthService
    {
        public readonly IAuthRepository _authRepository;
        public readonly IJwtTokenService _jwtTokenService;
        public AuthService(IAuthRepository authRepository, IJwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
        }
        public async Task<UserResponseDTO> LoginAsync(LoginRequestDTO request)
        {

            var repos = await _authRepository.Login(request.Email, request.HashedPassword);
            // rasie domain event

            ApplicationUser user = new ApplicationUser();
            user.loginDomainEvent();



            if (repos == null)
            {
                throw new Exception("Invalid email or password.");
            }
           
            return repos;   
        }

        public async Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            var repos = await _authRepository.Register(request.FirstName, request.Email, request.HashedPassword , request.Gender , request.Role);

            // rasie domain event

            ApplicationUser user = new ApplicationUser();
            user.registerDomainEvent();


            if (repos == null)
            {
                throw new Exception("Registration failed.");
            }
            return repos;
        }

        public async Task<string> RefreshToken()
        {
            return await _jwtTokenService.GenerateRefreshToken();
        }

    
        public async Task<RevokeRefreshTokenResponse> LogoutAsync(RefreshTokenRequest refreshTokenRemoveRequest)
        {
            return await _authRepository.RevokeRefreshToken(refreshTokenRemoveRequest);
        }
    }
}
