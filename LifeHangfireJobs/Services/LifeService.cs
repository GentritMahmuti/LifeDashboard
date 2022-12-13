using Amazon.Runtime;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Entities;
using LifeHangfireJobs.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace LifeHangfireJobs.Services
{
    public class LifeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public LifeService(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task NotifyAdmin()
        {
            var newlyCreatedOrdersIds = await _unitOfWork.Repository<OrderData>()
                                                        .GetByCondition(x => x.OrderStatus == StaticDetails.Created)
                                                        .Select(x => x.OrderId)
                                                        .ToListAsync();

            foreach (var orderId in newlyCreatedOrdersIds)
            {
                await _emailSender.SendEmailAsync("albion.b@gjirafa.com", "New order to check", $"New order created: {orderId}");
            }

        }

        public async Task GetMetrics()
        {


            
            var orderData = await _unitOfWork.Repository<OrderData>()
                                                   .GetAll()
                                                   .Include(x => x.User)
                                                   .ToListAsync();

            var orders = await _unitOfWork.Repository<OrderDetails>()
                                                                    .GetAll()
                                                                    .Include(x => x.Product)
                                                                    .Include(x => x.OrderData)
                                                                    .ToListAsync();
            //Request 1 : User that has most orders (including user data)
            var userIdWithMostOrders = orderData.GroupBy(orderData => orderData.UserId)
                                  .Select(x => new
                                  {
                                      UserId = x.Key,
                                      MaxNumberOfOrders = x.Count()
                                  }).OrderByDescending(x => x.MaxNumberOfOrders).FirstOrDefault();

            var userWithMostOrders = _unitOfWork.Repository<User>().GetAll().Where(x => x.Id == userIdWithMostOrders.UserId).FirstOrDefault();

            //Request 3 : The Day with the most orders
            var theDayWithTheMostOrders = orders.Select(x => x.OrderData.OrderDate)
                                        .GroupBy(i => i)
                                        .OrderByDescending(grp => grp.Count())
                                        .Select(grp => grp.Key)
                                        .First();


            //Request 4 : The Day with the least orders
            var theDayWithTheLeastOrders = orders.Select(x => x.OrderData.OrderDate)
                                        .GroupBy(i => i)
                                        .OrderByDescending(grp => grp.Count())
                                        .Select(grp => grp.Key)
                                        .Last();

            //Most expensive order
            var maxValue = orders.Max(x => x.Price);
            //Additional data for orders
            var mostExpensiveOrder = orders.First(x => x.Price == maxValue);
            //Cheapest order
            var minValue = orders.Min(x => x.Price);
            //Additional data for orders
            var cheapestOrder = orders.First(x => x.Price == minValue);


            var productsGroup = orders.GroupBy(x => x.Product)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Count).Sum());

            //Request 7 : Most sold product
            var mostSoldProduct = productsGroup.MaxBy(x => x.Value).Key;

            //Request 8 : Least sold product  (from those that have been sold)
            var leastSoldProduct = productsGroup.MinBy(x => x.Value).Key;

            //Request 9 : Displaing data that shows how many orders are there based on status
            var ordersByStatus = orderData.GroupBy(orderData => orderData.OrderStatus)
                                                    .Select(x => new
                                                    {
                                                        orderStatus = x.Key,
                                                        MaxOrders = x.Count()
                                                    }).ToList();


            //var model = new Metrics()
            //{
            //    MostSoldProduct = mostSoldProduct.Id,
            //    MostExpensiveOrder = (double)maxValue,
            //    CheapestOrder = (double)minValue,
            //    BestDay = bestDay
            //};








        }
    }
}
