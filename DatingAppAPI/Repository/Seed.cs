using DatingAppAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingAppAPI.Repository
{
    public class Seed
    {
        public static void SeedUsers(DatingAppContext db)
        {
            if (!db.User.Any())
            {
                var userData = System.IO.File.ReadAllText("Repository/User.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var item in users)
                {
                    byte[] PasswordHash, PasswordSalt;
                    CreatePasswordHash("password", out PasswordHash, out PasswordSalt);
                    item.PasswordHash = PasswordHash;
                    item.PasswordSalt = PasswordSalt;
                    item.UserName = item.UserName.ToLower();
                    db.User.Add(item);
                }
                db.SaveChanges();
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
