using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.Ultilities.HandleError;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DressUpExchange.Service.Services
{
    public interface ISendSSMSService
    {
        public Task<bool> ForgetPassword(string telephonenumber);

        public Task<bool> ConfirmPassword(int otpSending);

        public Task<bool> ChangePassword(string telephoneNumber, string newPassword);
    }
    public class SendSMSService : ISendSSMSService
    {
        private readonly IConfiguration _config;
        private readonly IMemoryCache _memory;
        private readonly IUnitOfWork _unitOfWork;

        public SendSMSService(IConfiguration config, IMemoryCache memory, IUnitOfWork unitOfWork)
        {
            _memory = memory;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ForgetPassword(string telephonenumber)
        {
           
            var checkphoneNumber = _unitOfWork.Repository<User>().Where(x => x.PhoneNumber ==  telephonenumber).FirstOrDefault();
            if(checkphoneNumber is null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Your phone number don't have in system");
            }
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string authToken = _unitOfWork.Repository<User>().Where(x => x.PhoneNumber == "0923581111" && x.Password == "comsuonhocmon").FirstOrDefault()?.Name ?? "1eef725eabb533d667aaa89b3556e24d";
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
            _memory.Set("phoneChangePassword", telephonenumber, cacheEntryOptions);
            _memory.Set("otpSending", randomNumber, cacheEntryOptions);
            TwilioClient.Init(_config["Twilio:AccountSid"], authToken);
            var messageOptions = new CreateMessageOptions(
            new PhoneNumber("+84392658221"));
            messageOptions.From = new PhoneNumber("+13144037625");
            messageOptions.Body = $"Your OTP to change password : {randomNumber}";

            var message = MessageResource.Create(messageOptions);

            return true;
        }


        public async Task<bool> ConfirmPassword(int otpSending)
        {
            _memory.TryGetValue("otpSending", out int otp);

            if (otp == otpSending)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
               .SetSlidingExpiration(TimeSpan.FromMinutes(5))
               .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
               .SetPriority(CacheItemPriority.Normal)
               .SetSize(1024);
                _memory.Set("checkOTPBefore", "Accepted", cacheEntryOptions);
                return true;
            };

            return false;
        }

        public async Task<bool> ChangePassword(string telephoneNumber, string newPassword)
        {
            _memory.TryGetValue("checkOTPBefore", out string? checkOtP);
            if (checkOtP is null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest,"Please enter you OTP first");
            }
            _memory.TryGetValue("phoneChangePassword", out string? TelephoneInChecking);

            
            if (TelephoneInChecking == telephoneNumber)
            {
                User userFind = await _unitOfWork.Repository<User>().GetAsync(x => x.PhoneNumber == telephoneNumber);
                userFind.Password = newPassword;
                await _unitOfWork.Repository<User>().Update(userFind, userFind.UserId);
                await _unitOfWork.CommitAsync();
                _memory.Remove("phoneChangePassword");
                _memory.Remove("checkOTPBefore");
                _memory.Remove("otpSending");
                return true;

            }
            else
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotAcceptable, "Not match your firstNumber");
            }
          
            return false;
        }
    }
}
