using Identity.Application.Contracts.Interface;
using Identity.Application.DTOs;
using Identity.Domain.Entities;

namespace Identity.Application.Contracts.Service
{
    public class AuthService : IAuthService
    {
        public readonly IAuthRepository _authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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
            var repos = await _authRepository.Register(request.FirstName, request.Email, request.HashedPassword);

            // rasie domain event

            ApplicationUser user = new ApplicationUser();
            user.registerDomainEvent();


            if (repos == null)
            {
                throw new Exception("Registration failed.");
            }
            return repos;
        }
    }
}
