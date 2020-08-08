using DatingAppAPI.DTO;
using DatingAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
    public class UserRepo : IUserRepo
    {
        DatingAppContext db;
        public UserRepo(DatingAppContext _db)
        {
            db = _db;
        }
        public async Task<int> AddUser(User user)
        {
            try
            {


                if (db != null)
                {
                    await db.User.AddAsync(user);
                    await db.SaveChangesAsync();

                    return user.Id;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return 0;
        }

        public async Task<UserDTO> GetUser(User user)
        {
            if (db != null)
            {
                return await (from p in db.User
                              where p.UserName == user.UserName && p.Password == user.Password
                              select new UserDTO
                              {
                                  Id = p.Id,
                                  UserName = p.UserName,
                                  Password = p.Password,
                              }).FirstOrDefaultAsync();
            }

            return null;
        }

    }
}
