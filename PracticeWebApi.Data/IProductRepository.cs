using PracticeWebApi.Data.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PracticeWebApi.Data
{
    public interface IProductRepository
    {
        Task<ProductDataEntity> AddProduct(ProductDataEntity product);

        /// <summary>
        /// This will return the <see cref="ProductDataEntity"/> if it exists despite <see cref="ProductDataEntity.IsActive"/> being true or false
        /// </summary>
        Task<ProductDataEntity> FindProductById(string productId);

        /// <summary>
        /// Finds all products from the products table
        /// </summary>
        Task<IList<ProductDataEntity>> GetAllProducts();

        /// <summary>
        /// Finds all products by <see cref="Product.GroupId"/> where <see cref="ProductDataEntity.IsActive"/> is true
        /// </summary>
        Task<IList<ProductDataEntity>> GetProductsByGroupId(string groupId);
        Task UpdateProduct(ProductDataEntity product);

        /// <summary>
        /// This method won't actually delete but set <see cref="ProductDataEntity.IsActive"/> to false
        /// </summary>
        Task DeactiveProduct(string productId);

        /// <summary>
        /// Sets <see cref="Product.IsActive"/> to true
        /// </summary>
        Task ActivateProduct(string productId);
    }
}
