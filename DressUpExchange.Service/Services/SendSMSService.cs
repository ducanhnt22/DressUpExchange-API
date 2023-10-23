﻿using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public SendSMSService(IConfiguration config, IMemoryCache memory,IUnitOfWork unitOfWork)
        {
            _memory = memory;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> ForgetPassword(string telephonenumber)
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string authToken =  _unitOfWork.Repository<User>().Where(x => x.PhoneNumber == "01235811" &&x.Password == "comsuonhocmon").FirstOrDefault()?.Name ?? "b79541969c35c7e9009dc11d18d46175";
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
            _memory.Set("phoneChangePassword", telephonenumber, cacheEntryOptions);
            _memory.Set("otpSending", randomNumber, cacheEntryOptions);
            TwilioClient.Init(_config["Twilio:AccountSid"],authToken);
            var messageOptions = new CreateMessageOptions(
            new PhoneNumber("+84392658221"));
            messageOptions.From = new PhoneNumber("+13144037625");
            messageOptions.Body = $"Your OTP to change password : {randomNumber}";

            var message = MessageResource.Create(messageOptions);

            return true;
        }


        public async Task<bool> ConfirmPassword(int otpSending)
        {
            _memory.TryGetValue("otpSending",out int otp);

            if(otp == otpSending) return true;

            return false;
        }

        public async Task<bool> ChangePassword(string telephoneNumber,string newPassword)
        {
            _memory.TryGetValue("phoneChangePassword", out string? TelephoneInChecking);

            if(TelephoneInChecking == telephoneNumber)
            {
                User userFind =  await _unitOfWork.Repository<User>().GetAsync(x => x.PhoneNumber == telephoneNumber);
                userFind.Password = newPassword;
               await _unitOfWork.Repository<User>().Update(userFind, userFind.UserId);
               await _unitOfWork.CommitAsync();
                return true;
            }

            return false;
        }
    }
}
