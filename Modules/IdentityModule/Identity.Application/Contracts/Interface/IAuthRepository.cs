using Identity.Application.DTOs;
using Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Contracts.Interface
{
    public interface IAuthRepository
    {
        Task<UserResponseDTO> Register(string name , string email , string password);

        Task<UserResponseDTO> Login(string email , string password);
    }
}
