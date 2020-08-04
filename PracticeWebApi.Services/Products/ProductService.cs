using PracticeWebApi.CommonClasses.Products;
using PracticeWebApi.Data;
using PracticeWebApi.Data.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticeWebApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDataEntity> _mapper;

        public ProductService(IProductRepository productRepository, IMapper<Product, ProductDataEntity> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task ActivateProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentNullException(nameof(productId));

            await _productRepository.ActivateProduct(productId);
        }

        public async Task<Product> AddProduct(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            product.Id = Guid.NewGuid().ToString();
            product.IsActive = false;

            return _mapper.MapToBase(await _productRepository.AddProduct(_mapper.MapToDataEntity(product)));
        }

        public async Task DeactiveProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentNullException(nameof(productId));

            await _productRepository.DeactiveProduct(productId);
        }

        public async Task<Product> FindProductById(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) throw new ArgumentNullException(nameof(productId));

            var productDataEntity = await _productRepository.FindProductById(productId);
            var product = _mapper.MapToBase(productDataEntity);

            return product;
        }

        //added to allow getting all products
        public async Task<IList<Product>> GetAllProducts()
        {
            var productDataEntities = await _productRepository.GetAllProducts();
            var products = productDataEntities.Select(p => _mapper.MapToBase(p));

            return products.ToList();
        }

        public async Task<IList<Product>> GetProductsByGroupId(string groupId)
        {
            if (string.IsNullOrWhiteSpace(groupId)) throw new ArgumentNullException(nameof(groupId));

            var productDataEntities = await _productRepository.GetProductsByGroupId(groupId);
            var products = productDataEntities.Select(p => _mapper.MapToBase(p));

            return products.ToList();
        }

        public async Task UpdateProduct(Product product)
        {
            if (product is null) throw new ArgumentNullException(nameof(product));

            await _productRepository.UpdateProduct(_mapper.MapToDataEntity(product));
        }
    }
}
