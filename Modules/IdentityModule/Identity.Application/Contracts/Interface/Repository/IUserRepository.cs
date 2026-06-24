using Identity.Application.DTOs;


namespace Identity.Application.Contracts.Interface.Repository
{
    public interface IUserRepository
    {
        Task<Guid> CreateUser(CreateUserDTO createUserDTO);
        Task UpdateUser(string userId ,UpdateUserDTO updateUserDTO);
        Task DeactivateUser(string UserId);
        Task ActivateUser(string UserId);
    }
}
