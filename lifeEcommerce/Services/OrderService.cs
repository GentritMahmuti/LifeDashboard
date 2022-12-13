using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Services.IService;
using AutoMapper;
using lifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using lifeEcommerce.Helpers;

namespace lifeEcommerce.Services
{
    public class OrderService : IOrderService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ProcessOrder(List<string> orderIds, string status)
        {
            var ordersToUpdate = await _unitOfWork.Repository<OrderData>()
                                                        .GetByCondition(x => orderIds.Contains(x.OrderId))
                                                        .ToListAsync();
            var carrier = string.Empty;
            if (status == StaticDetails.Shipped)
            {
                carrier = Guid.NewGuid().ToString();
            }

            foreach (var order in ordersToUpdate)
            {
                order.OrderStatus = status;
                order.Carrier = carrier;
            }

            _unitOfWork.Repository<OrderData>().UpdateRange(ordersToUpdate);

            _unitOfWork.Complete();
        }
    }
}
