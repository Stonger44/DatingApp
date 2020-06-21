using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _datingRepo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository datingRepo, IMapper mapper)
        {
            _datingRepo = datingRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _datingRepo.GetUsers();

            var usersToReturn = _mapper.Map<IEnumerable<UserListDTO>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingRepo.GetUser(id);

            var userToReturn = _mapper.Map<UserDetailedDTO>(user);

            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserEditDTO userEditDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _datingRepo.GetUser(id);

            _mapper.Map(userEditDTO, userFromRepo);

            if (await _datingRepo.SaveAll())
                return NoContent(); // '204 No Content' means save was successful

            throw new Exception($"Failed to update user {id}"); 
        }
    }
}