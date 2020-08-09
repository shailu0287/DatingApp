using DatingAppAPI.DTO;
using DatingAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
   public interface IAuthRepository
    {
        Task<int> AddUser(User user);
        Task<UserForRegisterDTO> GetUser(User user);
    }
}
