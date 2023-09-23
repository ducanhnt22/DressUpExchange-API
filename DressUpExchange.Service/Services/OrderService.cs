using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{
    public interface IOrderService
    {
        Task<bool> AddNewOrder(OrderRequest orderRequest);
        Task<GeneralOrderResponse> GetOrderByCustomer(int userID);
    }
    public class OrderService : IOrderService
    {
       
        private static IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddNewOrder(OrderRequest orderRequest)
        {

            DateTime dateTime = DateTime.Now;
            Order order = new Order()
            {

                TotalAmount = orderRequest.TotalAmount,
                OrderDate = dateTime,
                Status = orderRequest.Status,
                UserId = orderRequest.UserId
            };

            await _unitOfWork.Repository<Order>().CreateAsync(order);
            _unitOfWork.Commit();
            int orderItemNew = _unitOfWork.Repository<Order>().Find(x => x.OrderDate == dateTime).OrderId;
            foreach (var item in orderRequest.OrderItemsRequest)
            {
                OrderItem orderItem = new OrderItem()
                {
                    OrderId = orderItemNew,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UserSavedVoucherId = item.UserSavedVoucherId,
                    Status = OrderState.Processing.ToString()
                };
                await _unitOfWork.Repository<OrderItem>().CreateAsync(orderItem);
                _unitOfWork.Commit();
            }

            return true;

        }

        public async Task<GeneralOrderResponse> GetOrderByCustomer(int userID)
        {
            GeneralOrderResponse generalOrderResponse = await QueryFormat.getOrder(userID);
            
            return generalOrderResponse;
                                    
        }
    }
}
