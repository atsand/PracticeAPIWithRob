using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApi.Data.Products
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private string _connectionString = "Data Source = DESKTOP-1RVNMQE\\SQLEXPRESS; Initial Catalog = PracticeCommerce; Integrated Security = True;";
        private string _insertStatement = "INSERT INTO ProductGroups (Id, Name) VALUES (@Id, @Name)";
        private string _deleteStatement = "DELETE FROM ProductGroups WHERE [Id] = @Id";
        private string _getAllGroups = "SELECT * FROM ProductGroups";

        public async Task AddGroup(ProductGroupDataEntity productGroupDataEntity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(_insertStatement, productGroupDataEntity);
            }
        }

        public async Task DeleteProductGroup(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(_deleteStatement, new { Id = id });
            }
        }

        public async Task<IList<ProductGroupDataEntity>> GetAllProductGroups()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<ProductGroupDataEntity>(_getAllGroups);
                return results.ToList();
            }
        }
    }
}
