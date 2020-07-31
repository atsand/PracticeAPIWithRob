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
        private readonly DatabaseConfiguration _databaseConfiguration;

        private string _insertProduct = @"
                INSERT INTO Products (id, name, description, price, groupId, isActive) 
                VALUES (@Id, @Name, @Description, @Price, @GroupId, @IsActive)";
        private string _selectAllProducts = @"
                SELECT * FROM Products";
        private string _findProductById = @"
                SELECT * FROM Products
                WHERE [Id] = @Id";
        private string _findProductsByProductGroupId = @"
                SELECT * FROM Products
                WHERE [GroupId] = @GroupId";
        private string _updateProduct = @"
                UPDATE Products 
                SET [name] = @Name,
                    [description] = @Description,
                    [price] = @Price,
                    [groupId] = @GroupId
                WHERE [Id] = @Id";
        private string _activateProduct = @"
                UPDATE Products
                SET [isActive] = 1
                WHERE [id] = @Id";
        private string _deactivateProductById = @"
                UPDATE Products
                SET [isActive] = 0
                WHERE [id] = @Id";

        public ProductRepository(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        public async Task ActivateProduct(string productId)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                await connection.ExecuteAsync(_activateProduct, new { Id = productId });
            };
        }

        public async Task<ProductDataEntity> AddProduct(ProductDataEntity product)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                var products = await GetAllProducts();
                if (products.Any(existingProduct => existingProduct.Id == product.Id || existingProduct.Name == product.Name))
                {
                    throw new DuplicateResourceException($"A product already exists with Id {product.Id} or Name {product.Name}");
                }
                await connection.ExecuteAsync(_insertProduct, product);
                return await Task.FromResult(product);
            }
        }

        public async Task DeactiveProduct(string productId)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                await connection.ExecuteAsync(_deactivateProductById, new { Id = productId });
            }
        }

        public async Task<ProductDataEntity> FindProductById(string productId)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                var results = await connection.QueryAsync<ProductDataEntity>(_findProductById, new { Id = productId });

                if (!results.Any()) throw new ResourceNotFoundException($"No product found with id {productId}");
                if (results.Count() > 1) throw new DuplicateResourceException($"Multiple products found with id {productId}");

                return results.Single();
            }
        }

        public async Task<IList<ProductDataEntity>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                return (await connection.QueryAsync<ProductDataEntity>(_selectAllProducts)).ToList();
            }
        }

        public async Task<IList<ProductDataEntity>> GetProductsByGroupId(string groupId)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                var results = await connection.QueryAsync<ProductDataEntity>(_findProductsByProductGroupId, new { GroupId = groupId });
                if (!results.Any()) throw new ResourceNotFoundException($"No products found with groupID {groupId}");

                return results.ToList();
            }
        }

        public async Task UpdateProduct(ProductDataEntity product)
        {
            using (var connection = new SqlConnection(_databaseConfiguration.ConnectionString))
            {
                await FindProductById(product.Id);
                await connection.ExecuteAsync(_updateProduct, product);
            }
        }
    }
}
