using Microsoft.AspNetCore.Mvc;

namespace LifeHangfireJobs.Controllers
{
    public class MetricsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
