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
        public async Task<User> AddUser(User user,string password)
        {
            try
            {

            

                if (db != null)
                {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                    
                    await db.User.AddAsync(user);
                    await db.SaveChangesAsync();

                    return user;
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return null;
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
        public async Task<User> GetUser(string userName,string password)
        {
            if (db != null)
            {

                var user = await db.User.FirstOrDefaultAsync(x => x.UserName == userName);

                if (user == null)
                    return null;

                if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                         return null;

                    return user;
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

        public async Task<bool> UserExist(string userName)
        {
            if (db != null)
            { 
                var user= await (from p in db.User
                                where p.UserName == userName 
                                select new UserForRegisterDTO
                                {
                                    UserName = p.UserName,
                                    
                                }).FirstOrDefaultAsync();
                if (user != null)
                {
                    return true;
                }

            }
            return false;
        }
    }
}
