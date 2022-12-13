using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using lifeEcommerce.Models.Dtos.Unit;

namespace lifeEcommerce.Controllers
{
    [ApiController]
    public class CoverController : Controller
    {
        private readonly ICoverService _coverService;

        public CoverController(ICoverService coverService)
        {
            _coverService = coverService;
        }

        [Authorize]
        [HttpGet("GetCover")]
        public async Task<IActionResult> Get(int id)
        {
            var cover = await _coverService.GetCover(id);

            if (cover == null)
            {
                return NotFound();
            }

            return Ok(cover);
        }

        [HttpGet("GetCovers")]
        public async Task<IActionResult> GetCovers()
        {
            var covers = await _coverService.GetAllCovers();

            return Ok(covers);
        }

        [HttpPost("PostCover")]
        public async Task<IActionResult> Post(UnitCreateDto coverToCreate)
        {
            await _coverService.CreateCover(coverToCreate);

            return Ok("Cover created successfully!");
        }

        [HttpPut("UpdateCover")]
        public async Task<IActionResult> Update(UnitDto coverToUpdate)
        {
            await _coverService.UpdateCover(coverToUpdate);

            return Ok("Cover updated successfully!");
        }

        [HttpDelete("DeleteCover")]
        public async Task<IActionResult> Delete(int id)
        {
            await _coverService.DeleteCover(id);

            return Ok("Category deleted successfully!");
        }

    }
}
