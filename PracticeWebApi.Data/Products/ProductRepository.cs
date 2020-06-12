using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PracticeWebApi.Data.Base;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using PracticeWebApi.CommonClasses.Exceptions;
using System.Text.RegularExpressions;

namespace PracticeWebApi.Data.Products
{
    public class ProductRepository : IProductRepository
    {
        //could take this connection string up a level so it's accessible by all classes
        //private string _connectionString = new ConnectionConfiguration().GetConnectionString();
        private string _connectionString = "Data Source = .\\Web19; Initial Catalog = PracticeCommerce; Integrated Security = True;";
        private string _insertProduct = @"
                INSERT INTO Products
                            (id, name, description, price, groupId, isActive) 
                            VALUES (@Id, @Name, @Description, @Price, @GroupId, @IsActive)";
        private string _selectAllProducts = @"SELECT * FROM Products";
        private string _findProductById = @"SELECT * FROM Products WHERE [Id] = @Id";
        private string _findProductsByProductGroupId = @"SELECT * FROM Products WHERE [GroupId] = @ProductId";
        private string _updateProduct = @"UPDATE Products 
                SET [name] = @Name
                    [description] = @Description
                    [price] = @Price
                    [groupId] = @GroupId
                WHERE [Id] = @Id";
        private string _activateProduct = @"
                UPDATE Products
                SET [isActive] = 1,
                WHERE [id] = @Id";
        private string _deactivateProductById = @"
                UPDATE Products
                SET [isActive] = 0,
                WHERE [id] = @Id";

        //why use async on all calls?
        //when is the proper time to use async?
        public async Task ActivateProduct(string productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.ExecuteAsync(_activateProduct, new { Id = productId });
                };
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeactiveProduct(string productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.ExecuteAsync(_deactivateProductById, new { Id = productId });
                    //if ExecuteAsync returns number of rows, can you use 'if' statement to evaluate non-existing id?
                    //do you even need to do that logic to determine if it works or will it just throw an exception?
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ProductDataEntity> FindProductById(string productId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    //what is the difference between ExecuteAsync and QueryAsync?
                    //looks like QueryAsync returns the data and can be cast into a type with <class>
                    //ExecuteAsync returns a count of rows affected
                    var results = await connection.QueryAsync<ProductDataEntity>(_findProductById, new { Id = productId });

                    if (!results.Any()) throw new ResourceNotFoundException($"No product found with id {productId}");
                    if (results.Count() > 1) throw new DuplicateResourceException($"Multiple users found with id {productId}");

                    return results.Single();
                }
            }
            catch (Exception)
            {
                throw;
            }
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
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<ProductDataEntity>> GetProductsByGroupId(string groupId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var results = await connection.QueryAsync<ProductDataEntity>(_findProductsByProductGroupId, new { groupId = groupId });
                    if (!results.Any()) throw new ResourceNotFoundException($"No products found with groupID {groupId}");

                    return results.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateProduct(ProductDataEntity product)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await FindProductById(product.Id);
                    await connection.ExecuteAsync(_updateProduct, product);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
