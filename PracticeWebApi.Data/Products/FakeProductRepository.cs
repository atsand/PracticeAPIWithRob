using PracticeWebApi.CommonClasses.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApi.Data.Products
{
    public class FakeProductRepository : IProductRepository
    {
        private IList<ProductDataEntity> _products;

        public FakeProductRepository()
        {
            _products = new List<ProductDataEntity>();
        }

        public Task ActivateProduct(string productId)
        {
            var product = _products.Where(p => p.Id == productId).FirstOrDefault();

            if (product is null) throw new ResourceNotFoundException($"No Product with id {productId} found");

            product.IsActive = true;

            return Task.CompletedTask;
        }

        public Task<ProductDataEntity> AddProduct(ProductDataEntity product)
        {
            _products.Add(product);

            return Task.FromResult(product);
        }

        public Task DeactiveProduct(string productId)
        {
            var product = _products.Where(p => p.Id == productId).FirstOrDefault();

            if (product is null) throw new ResourceNotFoundException($"No Product with id {productId} found");

            product.IsActive = false;

            return Task.CompletedTask;
        }

        public Task<ProductDataEntity> FindProductById(string productId)
        {
            var product = _products.Where(p => p.Id == productId).FirstOrDefault();

            if (product is null) throw new ResourceNotFoundException($"No Product with id {productId} found");

            return Task.FromResult(product);
        }

        public Task<IList<ProductDataEntity>> GetAllProducts()
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<ProductDataEntity>> GetProductsByGroupId(string groupId)
        {
            IList<ProductDataEntity> products = _products.Where(p => p.GroupId == groupId && p.IsActive).ToList();

            return Task.FromResult(products);
        }

        public Task UpdateProduct(ProductDataEntity product)
        {
            _products = _products.Where(p => p.Id != product.Id).ToList();

            _products.Add(product);

            return Task.CompletedTask;
        }
    }
}
