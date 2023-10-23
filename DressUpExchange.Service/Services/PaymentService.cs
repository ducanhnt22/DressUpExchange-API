using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
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
