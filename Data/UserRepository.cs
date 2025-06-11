using MyApi.Models;
using System.Data;
using System.Data.SqlClient;


namespace MyApi.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("NextGenConString");
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Id, Name, Email FROM Users", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }

            return users;
        }

        public void AddUser(User user)
        {
            using SqlConnection conn = new SqlConnection(_connectionString);
            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Users (Name, Email) VALUES (@Name, @Email)", conn);

            cmd.Parameters.AddWithValue("@Name", user.Name);
            cmd.Parameters.AddWithValue("@Email", user.Email);

            cmd.ExecuteNonQuery();
        }
    }
}

