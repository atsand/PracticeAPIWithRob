using Dapper;
using PracticeWebApi.CommonClasses.Exceptions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApi.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private string _connectionString = "Data Source = .\\Web19; Initial Catalog = PracticeCommerce; Integrated Security = True;";
        private string _insertUser =
            @"INSERT INTO Users (id, firstName, lastName, email, address, city, state, zip) VALUES " +
            "(@Id, @FirstName, @LastName, @Email, @Address, @City, @State, @Zip)";
        private string _selectAllUsers = "SELECT * FROM Users";
        private string _findUserById = "SELECT * FROM Users WHERE [Id] = @Apples";
        private string _deleteUser = "DELETE FROM Users WHERE [Id] = @TunaFish";
        private string _updateUser = @"  
            UPDATE Users
              SET [firstName] = @FirstName,
	              [lastName] = @LastName,
	              [email] = @Email,
	              [address] = @Address,
	              [city] = @City,
	              [state] = @State,
	              [zip] = @Zip
            WHERE [Id] = @Id";

        public async Task UpdateUser(UserDataEntity userDataEntity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await FindUserById(userDataEntity.Id);
                await connection.ExecuteAsync(_updateUser, userDataEntity);
            }
        }

        public async Task DeleteUser(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await FindUserById(id);
                await connection.ExecuteAsync(_deleteUser, new { TunaFish = id });
            }
        }

        public async Task<UserDataEntity> FindUserById(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<UserDataEntity>(_findUserById, new { Apples = id });

                if (!results.Any()) throw new ResourceNotFoundException($"No user found with id {id}");
                if (results.Count() > 1) throw new DuplicateResourceException($"Multiple users found with id {id}");

                return results.Single();
            }
        }

        public async Task<IList<UserDataEntity>> GetAllUsers()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return (await connection.QueryAsync<UserDataEntity>(_selectAllUsers)).ToList();
            }
        }

        public async Task AddUser(UserDataEntity userDataEntity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var existingUsers = await GetAllUsers();
                if (existingUsers.Any(user => user.Id == userDataEntity.Id || user.Email == userDataEntity.Email))
                    throw new DuplicateResourceException($"A user already exists with Id {userDataEntity.Id} or Email {userDataEntity.Email}");

                await connection.ExecuteAsync(_insertUser, userDataEntity);
            }
        }
    }
}
