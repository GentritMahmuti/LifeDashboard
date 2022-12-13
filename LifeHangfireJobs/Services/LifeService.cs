using Amazon.Runtime;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Entities;
using LifeHangfireJobs.Dtos;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace LifeHangfireJobs.Services
{
    public class LifeService : ILifeService
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
                await _emailSender.SendEmailAsync("gentrit.mahmuti@life.gjirafa.com", "New order to check", $"New order created: {orderId}");
            }

        }

        public async Task<Metrics> GetMetrics()
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

            //Request 2 : User that spend most money
            var userIdWithMostMoneySpent = orderData.GroupBy(orderData => orderData.UserId)
                                                .Select(x => new
                                                {
                                                    UserId = x.Key,
                                                    TotalSpent = x.Sum(p => p.OrderTotal)
                                                }).OrderByDescending(x => x.TotalSpent).FirstOrDefault();

            var userWhoSpentMostMoney = _unitOfWork.Repository<User>().GetAll().Where(x => x.Id == userIdWithMostMoneySpent.UserId).FirstOrDefault();

            //Request 3 : The Day with the most orders
            var theDayWithTheMostOrders = orders.Select(x => x.OrderData.OrderDate)
                                        .GroupBy(i => i)
                                        .OrderByDescending(grp => grp.Count())
                                        .Select(grp => grp.Key)
                                        .FirstOrDefault();

            //Request 4 : The Day with the least orders
            var theDayWithTheLeastOrders = orders.Select(x => x.OrderData.OrderDate)
                                        .GroupBy(i => i)
                                        .OrderByDescending(grp => grp.Count())
                                        .Select(grp => grp.Key)
                                        .LastOrDefault();

            //Request 5 : Most expensive order (displaying data regarding it including product name, user name, count and price)
            var maxValue = orders.Max(x => x.Price);
            //Additional data for orders
            var mostExpensiveOrder = orders.First(x => x.Price == maxValue);
            OrderDto mostExpensiveOrderData = new OrderDto
            {
                ProductName = mostExpensiveOrder.Product.Title,
                UserName = mostExpensiveOrder.OrderData.User.FirsName,
                Count = mostExpensiveOrder.Count,
                Price = mostExpensiveOrder.Price
            };

            //Request 6 : Cheapest order (displaying data regarding it including product name, user name, count and price)
            var minValue = orders.Min(x => x.Price);
            //Additional data for orders
            var cheapestOrder = orders.First(x => x.Price == minValue);

            OrderDto cheapestOrderData = new OrderDto
            {
                ProductName = cheapestOrder.Product.Title,
                UserName = cheapestOrder.OrderData.User.FirsName,
                Count = cheapestOrder.Count,
                Price = cheapestOrder.Price
            };

            var productsGroup = orders.GroupBy(x => x.Product)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Count).Sum());

            //Request 7 : Most sold product
            var mostSoldProduct = productsGroup.MaxBy(x => x.Value).Key;

            //Request 8 : Least sold product  (from those that have been sold)
            var leastSoldProduct = productsGroup.MinBy(x => x.Value).Key;

            //Request 9 : Displaying data that shows how many orders are there based on status
            var ordersByStatusData = orderData.GroupBy(orderData => orderData.OrderStatus)
                                                    .Select(x => new
                                                    {
                                                        orderStatus = x.Key,
                                                        MaxOrders = x.Count()
                                                    }).ToList();

            Dictionary<string, int> ordersByStatus = new();
            foreach (var myOrder in ordersByStatusData)
            {
                ordersByStatus.TryAdd(myOrder.orderStatus, myOrder.MaxOrders);
            }

            var model = new Metrics()
            {
                UserWithMostOrders = userWithMostOrders,
                TheUserWithTheMostMoneySpent = userWhoSpentMostMoney,
                TheDayWithTheMostOrders = DateOnly.FromDateTime(theDayWithTheMostOrders),
                TheDayWithTheLeastOrders = DateOnly.FromDateTime(theDayWithTheLeastOrders),
                MostExpensiveOrder = mostExpensiveOrderData,
                CheapestOrder = cheapestOrderData,
                MostSoldProduct = mostSoldProduct,
                LeastSoldProductoperty = leastSoldProduct,
                OrdersByStatus = ordersByStatus
            };
            return model;
        }
    }
}
