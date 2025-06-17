using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Data;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _userRepository;

    public UsersController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        var id = await _userRepository.AddUserAsync(user);
        return Ok(new { id });
    }
}
