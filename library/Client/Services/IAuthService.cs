using grpcCrud;
using library.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace library.Client.Services
{
    public interface IAuthService
    {
        Task Registration(RegMsg r);
        Task Login(RegMsg r);
        Task Logout();
        Task<User> CheckUser();
    }
}
