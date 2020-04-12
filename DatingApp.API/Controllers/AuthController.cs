using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }


        [HttpPost("register")]
        // [FromBody] needed if not using [ApiController]
        // public async Task<IActionResult> Register([FromBody]UserRegisterDTO userRegisterDTO)
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            // // validate request - Needed if not using [ApiController]
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);

            userRegisterDTO.Username = userRegisterDTO.Username.ToLower();

            if (await _authRepo.UserExists(userRegisterDTO.Username))
                return BadRequest("Username already exists.");

            User userToCreate = new User
            {
                Username = userRegisterDTO.Username
            };

            User createdUser = await _authRepo.Register(userToCreate, userRegisterDTO.Password);

            //Temporary, in development
            return StatusCode(201);
        }
    }
}