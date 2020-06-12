using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracticeWebApi.CommonClasses.Exceptions;
using PracticeWebApi.CommonClasses.Products;
using PracticeWebApi.Services;
using System.Threading.Tasks;
using System;

namespace PracticeWebApi.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("/products")]
        public async Task<IActionResult> AddProduct([FromBody]Product product)
        {
            try
            {
                var addedProduct = await _productService.AddProduct(product);
                return Ok(addedProduct);
            }
            catch (DuplicateResourceException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpPost("/products/{productId}")]
        public async Task<IActionResult> ActivateProduct([FromRoute]string productId)
        {
            try
            {
                await _productService.ActivateProduct(productId);
                return Ok();
            }
            catch (DuplicateResourceException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpPut("/products")]
        public async Task<IActionResult> UpdateProduct([FromBody]Product product)
        {
            try
            {
                await _productService.UpdateProduct(product);
                return Ok();
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpDelete("/products/{productId}")]
        public async Task<IActionResult> DeactivateProduct([FromRoute]string productId)
        {
            try
            {
                await _productService.DeactiveProduct(productId);
                return Ok();
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("/products/{productId}")]
        public async Task<IActionResult> FindProductById([FromRoute]string productId)
        {
            try
            {
                var product = await _productService.FindProductById(productId);
                return Ok(product);
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }

        [HttpGet("/products/group/{groupId}")]
        public async Task<IActionResult> GetProductsByGroupId([FromRoute]string groupId)
        {
            try
            {
                var products = await _productService.GetProductsByGroupId(groupId);
                return Ok(products);
            }
            catch (ResourceNotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, exception.Message);
            }
        }
    }
}