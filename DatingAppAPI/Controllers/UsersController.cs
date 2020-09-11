using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Data;
using DatingAppAPI.Helpers;
using DatingApp.Data.Models;
using DatingApp.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DatingApp.Data.DTO;
using DatingApp.Data.Helper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatingAppAPI.Controllers
{
   // [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IDatingRepository userRepo;
        private readonly IConfiguration _config;
        private IMapper _mapper;
        public UsersController(IDatingRepository _userRepo, IConfiguration config, IMapper mapper)
        {
            userRepo = _userRepo;
            _config = config;
            _mapper = mapper;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
           
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = await userRepo.GetUser(userId);

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = user.Gender == "male" ? "female" : "male";
            }

            userParams.UserId = userId;
            var users = await userRepo.GetUsers(userParams);
            var userToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(userToReturn);
        }

        // GET api/<controller>/5
        [HttpGet("{id}", Name ="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userRepo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id,UserForEditProfileDTO user)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var userFromRepo = await userRepo.GetUser(id);
            _mapper.Map(user, userFromRepo);

            if (await userRepo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user failed on save");
        
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var like = await userRepo.GetLike(id, recipientId);

            if (like != null)
                return BadRequest("You alreday liked the user");
            if (await userRepo.GetUser(recipientId) == null)
                return NotFound();

            like = new Likes
            {
                LikeeId = recipientId,
                LikerId = id
            };

            userRepo.Add<Likes>(like);

            if (await userRepo.SaveAll())
                return Ok();

            return BadRequest("Failed to like user");
        }
    }
}
