using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyApi.Models;
using MyApi.Data;


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
        //var users = await _repo.GetAllUsersAsync();
        //return Ok(users);
        return Ok(new List<User> {
        new User { Id = 1, Name = "Test", Email = "test@example.com" }
        });
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
