using DatingAppAPI.DTO;
using DatingAppAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
   public interface IUserRepo
    {
        Task<int> AddUser(User user);
        Task<UserDTO> GetUser(User user);
    }
}
