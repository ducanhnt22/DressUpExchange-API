using AutoMapper;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Data.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DressUpExchange.Service.Exceptions;
using System.Net;
using AutoMapper.QueryableExtensions;
using DressUpExchange.Service.Ultilities;
using DressUpExchange.Service.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DressUpExchange.Service.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<UserResponse>> GetCustomers(CustomerRequest request, PagingRequest paging);
        Task<UserResponse> DeleteCustomer(int id);
        //Task<string> Verification(string phone, string code);
        //Task<UserResponse> ResetPassword(bool forgotPass, ResetPasswordRequest resetPassword, string email);
        Task<UserResponse> Login(LoginRequest request);
        Task<UserResponse> GetCustomerById(int id);
        Task<UserResponse> UpdateCustomer(int customerId, CustomerRequest request);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }
        public static bool CheckPhone(string phone)
        {
            string regex = @"(^(0)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$)"; //VN Number: example 0867952623
            Regex re = new Regex(regex);
            if (re.IsMatch(phone))
            {
                return true;
            } 
            else 
            return false;
        }
        public bool IsUniqueUser(string phone)
        {
            var user = _unitOfWork.Repository<User>().Find(u => u.PhoneNumber == phone);
            if (user == null)
            {
                return true;
            } else 
            return false;

        }
        public async Task<UserResponse> DeleteCustomer(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == id);
            try
            {
                if (user == null)
                {
                    throw new CrudException(System.Net.HttpStatusCode.NotFound, "Not found user with that id", id.ToString());
                }
                _unitOfWork.Repository<User>().Delete(user);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<User, UserResponse>(user);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Delete User Error!", ex.InnerException?.Message);
            }
        }

        public async Task<UserResponse> GetCustomerById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Id user invalid", "");
                }
                var response = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == id);
                if (response == null)
                {
                    throw new CrudException(System.Net.HttpStatusCode.NotFound, "Id user not found", response.UserId.ToString());
                }
                return _mapper.Map<UserResponse>(response);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get User By ID Error!!!", ex.InnerException?.Message);
            }
        }

        public Task<PagedResult<UserResponse>> GetCustomers(CustomerRequest request, PagingRequest paging)
        {
            var filter = _mapper.Map<UserResponse>(request);
            var customer = _unitOfWork.Repository<User>()
                                      .GetAll()
                                      .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                                      .DynamicFilter(filter)
                                      .ToList();
            var sort = PageHelper<UserResponse>.Sorting(paging.SortType, customer, paging.ColName);
            var result = PageHelper<UserResponse>.Paging(sort, paging.Page, paging.PageSize);
            return Task.FromResult(result);
        }
        private string GenerateJwtToken(User customer)
        {
            string role;
            if (customer.PhoneNumber.Equals(_config["AdminAccount:PhoneNumber"]) && customer.Password.Equals(_config["AdminAccount:Password"]))
                role = "admin";
            else role = "customer";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["ApiSetting:Secret"]);

            return null;
        }
        public Task<UserResponse> Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> UpdateCustomer(int customerId, CustomerRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
