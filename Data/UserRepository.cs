using MyApi.Models;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApi.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("SELECT Id, Name, Email FROM Users", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email"))
                });
            }

            return users;
        }

        public async Task<int> AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Use RETURNING Id to get inserted record's Id
            var query = "INSERT INTO Users (Name, Email) VALUES (@Name, @Email) RETURNING Id";

            await using var cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            var insertedId = (int)await cmd.ExecuteScalarAsync();

            return insertedId;
        }
    }
}
