using Microsoft.AspNetCore.Mvc;
using MyApi.Data;
using MyApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _repo;

        public UsersController(UserRepository repo)
        {
            _repo = repo;
        }
        [HttpGet("get")]
        public IActionResult MyGet() => Ok("Hello");

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _repo.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            if (user == null)
                return BadRequest("User data is required.");

            var id = await _repo.AddUserAsync(user);
            return Ok(new { message = "User added successfully.", userId = id });
        }
    }
}
