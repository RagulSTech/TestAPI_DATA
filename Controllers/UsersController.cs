using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyApi.Models;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly string _connectionString;

    public UsersController(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("❌ Connection string 'DefaultConnection' not found.");
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = new List<User>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand("SELECT id, name, email FROM users", conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "⚠️ Error fetching users", details = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        try
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var cmd = new NpgsqlCommand("INSERT INTO users (name, email) VALUES (@name, @email) RETURNING id", conn);
            cmd.Parameters.AddWithValue("name", user.Name);
            cmd.Parameters.AddWithValue("email", user.Email);

            var id = (int)await cmd.ExecuteScalarAsync();

            return Ok(new { id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "⚠️ Error adding user", details = ex.Message });
        }
    }
}
