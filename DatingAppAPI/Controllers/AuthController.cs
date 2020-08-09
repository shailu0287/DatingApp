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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingAppAPI.Controllers
{
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

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            try
            {
                var u = await userRepo.GetUser(user);

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
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddPost([FromBody] User model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = await userRepo.AddUser(model);
                    if (userId > 0)
                    {
                        return Ok(userId);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        private async Task<string> GenerateJwtToken(UserForRegisterDTO user)
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
