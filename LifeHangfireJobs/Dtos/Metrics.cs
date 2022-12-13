using lifeEcommerce.Models.Entities;
using System.Reflection.Metadata.Ecma335;

namespace LifeHangfireJobs.Dtos
{
    public class Metrics
    {

        public User UserWithMostOrders { get; set; }
        public User TheUserWithTheMostMoneySpent{ get; set; }
        public DateOnly TheDayWithTheMostOrders { get; set; }
        public DateOnly TheDayWithTheLeastOrders { get; set; }
        public OrderDto MostExpensiveOrder { get; set; }
        public OrderDto CheapestOrder { get; set; }
        public Product MostSoldProduct { get; set; }
        public Product LeastSoldProductoperty { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; }

    }
}
