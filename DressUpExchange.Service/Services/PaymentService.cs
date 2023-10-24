using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Exceptions;
using DressUpExchange.Service.Ultilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace DressUpExchange.Service.Services
{
    public interface IPaymentService
    {
        Task<string> PaymentAsync(decimal totalAmount);
        Task<string> OrderPaymentAsync(OrderRequest req);
        bool CheckQuantityProduct(int? productId);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IClaimsService _claimsService;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, IClaimsService claimsService, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _claimsService = claimsService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> OrderPaymentAsync(OrderRequest req)
        {
            var hash_secret = _configuration["VNPay:vnp_HashSecret"];
            var code = _configuration["VNPay:vnp_TmnCode"];
            var url = _configuration["VNPay:vnp_Url"];
            var return_url = _configuration["VNPay:vnp_Returnurl"];

            var userId = _claimsService.GetCurrentUserId;
            var dateTime = DateTime.UtcNow.AddHours(7);

            #region Create New Order
            Order order = new Order()
            {
                UserId = userId,
                TotalAmount = req.TotalAmount,
                Status = req.Status,
                OrderDate = req.OrderDate
            };
            
            await _unitOfWork.Repository<Order>().CreateAsync(order);
            await _unitOfWork.CommitAsync();

            List<Order> newOrderItem = await _unitOfWork.Repository<Order>().GetWhere(x => x.OrderDate == dateTime);
            Order lastOrder = newOrderItem.OrderByDescending(x => x.OrderDate).FirstOrDefault();
            int newOrderItemId = lastOrder.OrderId;

            foreach (var item in req.OrderItemsRequest)
            {
                await UpdateQuantityProduct(item.ProductId, item.Quantity);
                OrderItem orderItem = new OrderItem();
                orderItem.Quantity = item.Quantity;
                orderItem.ProductId = item.ProductId; orderItem.OrderId = newOrderItemId;
                orderItem.Status = OrderState.Processing.ToString();
                orderItem.VoucherId = item.VoucherId;
                orderItem.LaundryId = item.LaundryId;
                orderItem.Price = item.Price.ToString();
                await _unitOfWork.Repository<OrderItem>().CreateAsync(orderItem);
                _unitOfWork.Commit();
            }
            #endregion

            string formatDate = $"{order.OrderDate:yyyyMMddHHmmss}";

            VnPayLibrary lib = new VnPayLibrary();
            lib.AddRequestData("vnp_Version", "2.1.0");
            lib.AddRequestData("vnp_Command", "pay");
            lib.AddRequestData("vnp_TmnCode", code);

            lib.AddRequestData("vnp_Amount", (order.TotalAmount * 100).ToString());
            lib.AddRequestData("vnp_BankCode", "NCB");
            lib.AddRequestData("vnp_CreateDate", formatDate);
            lib.AddRequestData("vnp_CurrCode", "VND");
            lib.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            lib.AddRequestData("vnp_Locale", "vn");
            lib.AddRequestData("vnp_OrderInfo", "aaaa");
            lib.AddRequestData("vnp_OrderType", "other");
            lib.AddRequestData("vnp_ReturnUrl", return_url);
            lib.AddRequestData("vnp_TxnRef", formatDate);

            string fin = lib.CreateRequestUrl(url, hash_secret);
            return fin;
        }
        public bool CheckQuantityProduct(int? productId)
        {
            Product? product = _unitOfWork.Repository<Product>().Where(x => x.ProductId == productId).FirstOrDefault() ?? null;
            if (product.Quantity == 0)
            {
                return false;
            } return true;
        }
        private async Task<bool> UpdateQuantityProduct(int? productId, int? quantity)
        {
            Product? product = _unitOfWork.Repository<Product>().Where(x => x.ProductId == productId).FirstOrDefault() ?? null;
            if (product == null)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Not Found Product", "");

            }
            if (product.Quantity == 0)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Sold out", product.ProductId.ToString());
            }
            product.Quantity -= quantity;
            await _unitOfWork.Repository<Product>().Update(product, (int)productId);
            _unitOfWork.Commit();
            return true;
        }
        public async Task<string> PaymentAsync(decimal totalAmount)
        {
            var hash_secret = _configuration["VNPay:vnp_HashSecret"];
            var code = _configuration["VNPay:vnp_TmnCode"];
            var url = _configuration["VNPay:vnp_Url"];
            var return_url = _configuration["VNPay:vnp_Returnurl"];

            var dateTime = DateTime.UtcNow.AddHours(7);

            VnPayLibrary lib = new VnPayLibrary();

            lib.AddRequestData("vnp_Version", "2.1.0");
            lib.AddRequestData("vnp_Command", "pay");
            lib.AddRequestData("vnp_TmnCode", code);

            lib.AddRequestData("vnp_Amount", (totalAmount).ToString());
            lib.AddRequestData("vnp_BankCode", "NCB");
            lib.AddRequestData("vnp_CreateDate", dateTime.ToString("yyyyMMddHHmmss"));
            lib.AddRequestData("vnp_CurrCode", "VND");
            lib.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            lib.AddRequestData("vnp_Locale", "vn");
            lib.AddRequestData("vnp_OrderInfo", "aaaa");
            lib.AddRequestData("vnp_OrderType", "other");
            lib.AddRequestData("vnp_ReturnUrl", return_url);
            lib.AddRequestData("vnp_TxnRef", dateTime.ToString("yyyyMMddHHmmss"));

            string fin = lib.CreateRequestUrl(url, hash_secret);
            return fin;
        }


    }
}
