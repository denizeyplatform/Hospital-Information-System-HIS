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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Guid> CreateUser(CreateUserDTO createUserDTO)
        {
            return await _userRepository.CreateUser(createUserDTO);
       
        }
        public async Task UpdateUser(string userId, UpdateUserDTO updateUserDTO)
        {
            await _userRepository.UpdateUser(userId, updateUserDTO);
        }
        public async Task DeactivateUser(string UserId)
        {
            await _userRepository.DeactivateUser(UserId);
        }
        public async Task ActivateUser(string UserId)
        {
            await _userRepository.ActivateUser(UserId);
        }
    }
}
