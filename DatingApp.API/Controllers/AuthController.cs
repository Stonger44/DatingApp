using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository authRepo, IConfiguration config)
        {
            _authRepo = authRepo;
            _config = config;
        }


        [HttpPost("register")]
        #region [FromBody]
        // [FromBody] needed if not using [ApiController]
        // public async Task<IActionResult> Register([FromBody]UserRegisterDTO userRegisterDTO)
        #endregion
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            #region Validate ModelState
            // // validate request - Needed if not using [ApiController]
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);
            #endregion

            userRegisterDTO.Username = userRegisterDTO.Username.ToLower();

            if (await _authRepo.UserExists(userRegisterDTO.Username))
                return BadRequest("Username already exists.");

            User userToCreate = new User
            {
                Username = userRegisterDTO.Username
            };

            User createdUser = await _authRepo.Register(userToCreate, userRegisterDTO.Password);

            // Temporary, in development
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            User userFromRepo = await _authRepo.Login(userLoginDTO.Username.ToLower(), userLoginDTO.Password);

            if (userFromRepo == null)
                return Unauthorized();

            /*--------------------Create a Token--------------------*/
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token =  tokenHandler.CreateToken(tokenDescriptor);
            /*--------------------Create a Token--------------------*/

            // Want to return the token as an object to the client
            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}