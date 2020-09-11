using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Data;
using DatingApp.Data.DTO;
using DatingApp.Data.Models;
using DatingApp.Data.Repository;
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
        private IMapper _mapper;
        public AuthController(IAuthRepository _userRepo, IConfiguration config, IMapper mapper)
        {
            userRepo = _userRepo;
            _config = config;
            _mapper = mapper;
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
            var userToReturn = _mapper.Map<UserForListDTO>(u);
            if (u == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(new
                {
                    token = GenerateJwtToken(u).Result,
                    userToReturn
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

                var userToCreate = _mapper.Map<User>(model);
                var createdUser = await userRepo.AddUser(userToCreate, model.Password);
                var userToReturn = _mapper.Map<UserForDetailedDTO>(createdUser);
                if (userToCreate.Id > 0)
                {
                    return CreatedAtRoute("GetUser", new { Controller="Users", id= userToCreate.Id } ,userToReturn);
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
