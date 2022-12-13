using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System.Formats.Asn1;
using System.Globalization;
using System;
using CsvHelper;
using lifeEcommerce.Models.Entities;

namespace lifeEcommerce.Controllers
{
    public class UploadFromCSVController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public UploadFromCSVController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpPost("CreateMultipleCategoriesFromFile")]
        public async Task<IActionResult> PostMultipleCategoriesFromCSVFile(IFormFile file)
        {
            dynamic y = null;
            var categories = (dynamic)y;
            using (var reader = new StreamReader("Categories.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                 categories = csv.GetRecords<Category>();
            }
            _categoryService.CreateMultipleCategories(categories);

            return Ok(categories);
        }
        [HttpPost("CreateMultipleProductsFromFile")]
        public async Task<IActionResult> PostMultipleProductsFromCSVFile(IFormFile file)
        {
            dynamic y = null;
            var products = (dynamic)y;
            using (var reader = new StreamReader("Categories.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                products = csv.GetRecords<Product>();
            }
            _categoryService.CreateMultipleCategories(products);

            return Ok(products);
        }
        [HttpPost("CreateMultipleOrderDataFromFile")]
        public async Task<IActionResult> PostMultipleOrderDataFromCSVFile(IFormFile file)
        {
            dynamic y = null;
            var orderData = (dynamic)y;
            using (var reader = new StreamReader("OrderData.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
               orderData = csv.GetRecords<OrderData>();
            }
            _categoryService.CreateMultipleCategories(orderData);

            return Ok(orderData);
        }
        public async Task<IActionResult> PostMultipleOrderDetailsFromCSVFile(IFormFile file)
        {
            dynamic y = null;
            var orderDetails = (dynamic)y;
            using (var reader = new StreamReader("OrderDetails.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                orderDetails = csv.GetRecords<OrderDetails>();
            }
            _categoryService.CreateMultipleCategories(orderDetails);

            return Ok(orderDetails);
        }
        public async Task<IActionResult> PostMultipleUnitsFromCSVFile(IFormFile file)
        {
            dynamic y = null;
            var units = (dynamic)y;
            using (var reader = new StreamReader("Units.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                 units = csv.GetRecords<Unit>();
            }
            _categoryService.CreateMultipleCategories(units);

            return Ok(units);
        }
    }
}
