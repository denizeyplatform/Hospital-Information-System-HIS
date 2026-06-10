using Identity.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Interface.Repository
{
    public interface IAuthRepository
    {
        Task<object> Register(string name , string email , string password);

        Task<object> Login(string email , string password);
    }
}
