using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeWebApi.Data.Base;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using PracticeWebApi.CommonClasses.Exceptions;

namespace PracticeWebApi.Data.Products
{
    public class ProductRepository : IProductRepository
    {
        private string _connectionString = new ConnectionConfiguration().GetConnectionString();
        private string _insertProduct = @"
                INSERT INTO Products
                            (id, name, description, price, groupId, isActive) 
                            VALUES (@Id, @Name, @Description, @Price, @GroupId, @IsActive)";
        private string _selectAllProducts = "SELECT * FROM Products";
        private string _findProductById = "SELECT * FROM Products WHERE [Id] = @Id";
        private string _deleteProductById = "DELETE FROM Products WHERE [Id} = @Id";
        private string _updateProduct = @"UPDATE Products" +
                "SET [name] = @Name" +
                    "[description] = @Description" + 
                    "[price] = @Price" + 
                    "[groupId] = @GroupId" +
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
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.ExecuteAsync(_activateProduct, new { Id = productId });
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<ProductDataEntity> AddProduct(ProductDataEntity product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var products = await GetAllProducts();
                    if (products.Any(existingProduct => existingProduct.Id == product.Id || existingProduct.Name == product.Name))
                    {
                        throw new DuplicateResourceException($"A product already exists with Id {product.Id} or Email {product.Name}");
                    }
                    await connection.ExecuteAsync(_insertProduct, product);
                    return await Task.FromResult(product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public Task DeactiveProduct(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductDataEntity> FindProductById(string productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ProductDataEntity>> GetAllProducts()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return (await connection.QueryAsync<ProductDataEntity>(_selectAllProducts)).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
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
