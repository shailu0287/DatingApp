using DatingApp.Data;
using DatingAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
   public interface IAuthRepository
    {
        Task<User> AddUser(User user,string password);
        Task<User> GetUser(string userName,string password);

        Task<bool> UserExist(string userName);
    }
}
