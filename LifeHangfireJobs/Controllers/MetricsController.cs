using LifeHangfireJobs.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeHangfireJobs.Controllers
{
    public class MetricsController : Controller
    {
        private readonly ILifeService _lifeService;
        public MetricsController(ILifeService lifeService)
        {
            _lifeService = lifeService;
        }
        
        [HttpGet("Get Metrics")]
        public async Task<IActionResult> GetMetrics()
        {
            var metrics = _lifeService.GetMetrics();
            return Ok(metrics);
        }

    }
}
