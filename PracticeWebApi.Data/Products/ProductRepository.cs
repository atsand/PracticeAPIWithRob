using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeWebApi.Data.Base;
using System.Data.SqlClient;
using Dapper;

namespace PracticeWebApi.Data.Products
{
    public class ProductRepository : IProductRepository
    {
        private string _connectionString = new ConnectionConfiguration().GetConnectionString();
        private string _insertProduct = "INSERT INTO Products (id, name, description, price, groupId, isActive) " +
            "VALUES (@Id, @Name, @Description, @Price, @GroupId, @IsActive)";
        private string _selectAllProducts = "SELECT * FROM Products";
        private string _findProductById = "SELECT * FROM Products WHERE [Id] = @Id";
        private string _deleteProductById = "DELETE FROM Products WHERE [Id} = @Id";
        private string _updateProduct = @"UPDATE Products" +
                "SET [name] = @Name" +
                    "[description] = @Description" + 
                    "[price] = @Price" + 
                    "[groupId] = @GroupId" +
                    "[isActive] = @IsActive," +
            "WHERE [Id] = @Id";
        private string _activateProduct = @"
                UPDATE Products
                SET [isActive] = 1,
                WHERE [id] = @Id";
        private string _deactivateProduct = @"
                UPDATE Products
                SET [isActive] = 0,
                WHERE [id] = @Id";


        public async Task ActivateProduct(string productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await FindProductById(productId);
                await connection.ExecuteAsync(_activateProduct, );
            };
                throw new NotImplementedException();
        }

        public Task<ProductDataEntity> AddProduct(ProductDataEntity product)
        {
            throw new NotImplementedException();
        }

        public Task DeactiveProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDataEntity> FindProductById(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ProductDataEntity>> GetProductsByGroupId(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(ProductDataEntity product)
        {
            throw new NotImplementedException();
        }
    }
}
