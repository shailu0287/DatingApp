using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingAppAPI.DTO;
using DatingAppAPI.Models;
using DatingAppAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthRepository userRepo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository _userRepo, IConfiguration config)
        {
            userRepo = _userRepo;
            _config = config;
        }

        [HttpPost]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser([FromBody] UserForLoginDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

         

            var u = await userRepo.GetUser(user.userName, user.password);

            if (u == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new
                {
                    token = GenerateJwtToken(u).Result,
                    user = u
                });
            }

        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(UserForRegisterDTO model)
        {
            if (ModelState.IsValid)
            {

                if (await userRepo.UserExist(model.UserName))
                {
                    throw new Exception("User Already Exist");
                }

                var user = new User
                {
                    UserName = model.UserName
                };


                var userToCreate = await userRepo.AddUser(user, model.Password);
                if (userToCreate.Id > 0)
                {
                    return Ok(userToCreate.Id);
                }
            }
          
                return BadRequest();
            

        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };



            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
