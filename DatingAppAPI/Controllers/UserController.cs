using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingAppAPI.Models;
using DatingAppAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserRepo userRepo;
        public UserController(IUserRepo _userRepo)
        {
            userRepo = _userRepo;
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
                    return NotFound();
                }

                return Ok(u);
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
    }
}
