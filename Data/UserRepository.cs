using MyApi.Models;
using Npgsql;
using System.Data;

namespace MyApi.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }


        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT Id, Name, Email FROM Users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
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

        public void AddUser(User user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)", conn);

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            cmd.ExecuteNonQuery();
        }
    }
}
