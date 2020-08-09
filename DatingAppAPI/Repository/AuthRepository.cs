using DatingAppAPI.DTO;
using DatingAppAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        DatingAppContext db;
        public AuthRepository(DatingAppContext _db)
        {
            db = _db;
        }
        public async Task<int> AddUser(User user)
        {
           

                if (db != null)
                {
                    
                    await db.User.AddAsync(user);
                    await db.SaveChangesAsync();

                    return user.Id;
                }
          

            return 0;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }
        public async Task<UserForRegisterDTO> GetUser(User user)
        {
            if (db != null)
            {
                return await (from p in db.User
                              where p.UserName == user.UserName && p.Password == user.Password
                              select new UserForRegisterDTO
                              {
                                  Id = p.Id,
                                  UserName = p.UserName,
                                  Password = p.Password,
                              }).FirstOrDefaultAsync();
            }

            return null;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
