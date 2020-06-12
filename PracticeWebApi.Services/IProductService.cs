using PracticeWebApi.CommonClasses.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PracticeWebApi.Services
{
    public interface IProductService
    {
        Task<Product> AddProduct(Product product);

        /// <summary>
        /// This will return the <see cref="Product"/> if it exists despite <see cref="Product.IsActive"/> being true or false
        /// </summary>
        Task<Product> FindProductById(string productId);

        /// <summary>
        /// Finds all products by <see cref="Product.GroupId"/> despite <see cref="Product.IsActive"/> being true or false
        /// </summary>
        Task<IList<Product>> GetProductsByGroupId(string groupId);
        Task UpdateProduct(Product product);

        /// <summary>
        /// This will return list of all <see cref="Product"/> despite <see cref="Product.IsActive"/> being true or false
        /// </summary>
        Task<IList<Product>> GetAllProducts();

        /// <summary>
        /// This method won't actually delete but set <see cref="Product.IsActive"/> to false
        /// </summary>
        Task DeactiveProduct(string productId);

        /// <summary>
        /// Sets <see cref="Product.IsActive"/> to true
        /// </summary>
        Task ActivateProduct(string productId);
    }
}
