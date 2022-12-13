using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3.Model;
using Amazon.S3;
using lifeEcommerce.Models.Dtos.Product;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;

        public ProductController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
        }


        [HttpGet("GetProduct")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("GetProductWithIncludes")]
        public async Task<IActionResult> GetIncludes(int id)
        {
            var product = await _productService.GetWithIncludes(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var categories = await _productService.GetAllProducts();

            return Ok(categories);
        }

        [HttpGet("ProductsListView")]
        public async Task<IActionResult> ProductsListView(string? search, int categoryId = 0, int page = 1, int pageSize = 10)
        {
            var products = await _productService.ProductsListView(search ,page, pageSize, categoryId);

            return Ok(products);
        }

        [HttpPost("PostProduct")]
        public async Task<IActionResult> Post([FromForm] ProductCreateDto ProductToCreate)
        {
            await _productService.CreateProduct(ProductToCreate);

            return Ok("Product created successfully!");
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> Update(ProductDto ProductToUpdate)
        {
            await _productService.UpdateProduct(ProductToUpdate);

            return Ok("Product updated successfully!");
        }

        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);

            return Ok("Product deleted successfully!");
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var uploadPicture = await UploadToBlob(file);

            var imageUrl = $"{_configuration.GetValue<string>("BlobConfig:CDNLife")}{file.FileName + Path.GetExtension(file.FileName)}";
            
            return Ok(imageUrl);
        }

        [NonAction]
        public async Task<PutObjectResponse> UploadToBlob(IFormFile file)
        {

            //var model = _configuration.GetSection(nameof(BlobConfig)).Get<BlobConfig>();

            string serviceURL = _configuration.GetValue<string>("BlobConfig:serviceURL");
            string AWS_accessKey = _configuration.GetValue<string>("BlobConfig:accessKey");
            string AWS_secretKey = _configuration.GetValue<string>("BlobConfig:secretKey");
            var bucketName = _configuration.GetValue<string>("BlobConfig:bucketName");
            var keyName = _configuration.GetValue<string>("BlobConfig:defaultFolder");

            var config = new AmazonS3Config() { ServiceURL = serviceURL };
            var s3Client = new AmazonS3Client(AWS_accessKey, AWS_secretKey, config);
            keyName = String.Concat(keyName,file.FileName);

            var fs = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = keyName,
                InputStream = fs,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            return await s3Client.PutObjectAsync(request);
        }

    }
}
