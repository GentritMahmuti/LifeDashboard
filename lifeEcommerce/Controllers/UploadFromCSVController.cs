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
            var categories = null ;
            using (var reader = new StreamReader("Categories.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                 categories = csv.GetRecords<Category>();
            }
            _categoryService.CreateMultipleCategories(categories);

            return Ok(categories);
        }
        [HttpPost("CreateMultipleQuestionsFromFile")]
        public async Task<IActionResult> PostMultipleProductsFromCSVFile(IFormFile file)
        {
            using (var reader = new StreamReader("Products.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Product>();
            }

            return Ok();
        }
    }
}
