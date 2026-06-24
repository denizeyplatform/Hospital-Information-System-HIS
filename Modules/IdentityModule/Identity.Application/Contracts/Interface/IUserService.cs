using Identity.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IUserService
    {
        Task<Guid> CreateUser(CreateUserDTO createUserDTO);
        Task UpdateUser(string userId, UpdateUserDTO updateUserDTO);
        Task DeactivateUser(string UserId);
        Task ActivateUser(string UserId);
    }
}
