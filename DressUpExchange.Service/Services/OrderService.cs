using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Exceptions;
using DressUpExchange.Service.Helpers;
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
        Task<GeneralOrderResponse> GetOrderByCustomer(int userID,OrderPagingRequest orderPaging);
    }
    public class OrderService : IOrderService
    {
        private static IClaimsService _claimService;
        private static IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork,IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _claimService = claimsService;
        }

        public async Task<bool> AddNewOrder(OrderRequest orderRequest)
        {

            DateTime dateTime = DateTime.Now;
            Order order = new Order()
            {

                TotalAmount = orderRequest.TotalAmount,
                OrderDate = dateTime,
                Status = orderRequest.Status,
                UserId = _claimService.GetCurrentUserId
            };

            await _unitOfWork.Repository<Order>().CreateAsync(order);
            _unitOfWork.Commit();
            int orderItemNew = _unitOfWork.Repository<Order>().Find(x => x.OrderDate == dateTime).OrderId;
            foreach (var item in orderRequest.OrderItemsRequest)
            {
                await UpdateQuantityProduct(item.ProductId, item.Quantity);
                OrderItem orderItem = new OrderItem();
                orderItem.Quantity = item.Quantity;
                orderItem.ProductId = item.ProductId; orderItem.OrderId = orderItemNew;
                orderItem.Status = OrderState.Processing.ToString();
                orderItem.UserSavedVoucherId = item.UserSavedVoucherId;
                await _unitOfWork.Repository<OrderItem>().CreateAsync(orderItem);
                _unitOfWork.Commit();
            }

            return true;

        }

        public async Task<GeneralOrderResponse> GetOrderByCustomer(int userID, OrderPagingRequest orderPaging)
        {
            var generalOrderResponse = await QueryFormat.getOrder(userID,orderPaging.Status,orderPaging.Page,orderPaging.PageSize);
   
            
            return generalOrderResponse;
                                    
        }

        public async Task<bool> UpdateQuantityProduct(int? productId,int? quantity)
        {
            Product? product =   _unitOfWork.Repository<Product>().Where(x => x.ProductId == productId).FirstOrDefault() ?? null;
            if (product == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Not Found Product", "");

            }
            if(product.Quantity == 0)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Sold out", product.ProductId.ToString());
            }
            product.Quantity = quantity;
          await  _unitOfWork.Repository<Product>().Update(product, (int)productId);    
            _unitOfWork.Commit();
            return true;
        }
    }
}
