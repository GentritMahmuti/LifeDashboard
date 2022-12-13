using LifeHangfireJobs.Dtos;

namespace LifeHangfireJobs.Services
{
    public interface ILifeService
    {
        Task<Metrics> GetMetrics();
    }
}
