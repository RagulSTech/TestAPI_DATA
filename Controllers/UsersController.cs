using Microsoft.AspNetCore.Mvc;
using MyApi.Data;
using MyApi.Models;

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

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _repo.GetAllUsers();
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            _repo.AddUser(user);
            return Ok(new { message = "User added successfully." });
        }
    }
}
