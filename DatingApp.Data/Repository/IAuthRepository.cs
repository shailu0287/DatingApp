using DatingApp.Data.DTO;
using DatingApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data.Repository
{
   public interface IAuthRepository
    {
        Task<User> AddUser(User user,string password);
        Task<User> GetUser(string userName,string password);

        Task<bool> UserExist(string userName);
    }
}
