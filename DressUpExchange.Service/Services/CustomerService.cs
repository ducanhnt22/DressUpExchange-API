using AutoMapper;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Data.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
using System.Numerics;

namespace DressUpExchange.Service.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<UserResponse>> GetCustomers(CustomerRequest request, PagingRequest paging);
        Task<UserResponse> DeleteCustomer(int id);
        //Task<string> Verification(string phone, string code);
        //Task<UserResponse> ResetPassword(bool forgotPass, ResetPasswordRequest resetPassword, string email);
        Task<UserLoginResponse> LoginAsync(LoginRequest request);
        Task<UserResponse> GetCustomerById(int id);
        Task<UserResponse> UpdateCustomer(int customerId, CustomerRequest request);
        Task<UserResponse> RegisterAsync(RegisterRequest request);
        Task<UserResponse> UpdateAsync(int id, UserRequest request);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken);
        bool CheckPhone(string phone);
        bool IsUniqueUser(string phone);
        bool CheckPassword(User user, string password);
        Task<User> GetCustomerByPhone(string phone);
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
        public bool CheckPhone(string phone)
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
            }
            else
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
           // var sort = PageHelper<UserResponse>.Sorting(paging.SortType, customer, paging.ColName).ToList();
            var result = PageHelper<UserResponse>.Paging(customer, paging.Page, paging.PageSize);
            return Task.FromResult(result);
        }
        public async Task<UserLoginResponse> LoginAsync(LoginRequest request)
        {
            #region checkPhone
            var check = CheckPhone(request.Phone);
            //if (check)
            //{
            //    if (!request.Phone.StartsWith("+84"))
            //    {
            //        request.Phone = request.Phone.TrimStart(new char[] { '0' });
            //        request.Phone = "+84" + request.Phone;
            //    }
            //}
            //else
            //{
            //    throw new CrudException(HttpStatusCode.BadRequest, "Wrong Phone Format", request.Phone.ToString());
            //}
            #endregion

            var user = await _unitOfWork.Repository<User>()
                                        .Where(u => u.PhoneNumber == request.Phone && u.Password == request.Password)
                                        .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new CrudException(HttpStatusCode.NotFound, "Phone not found", request.Phone.ToString());
            }

            string secretKeyConfig = _config["JWTSecretKey:SecretKey"];
            DateTime secretKeyDateTime = DateTime.UtcNow;
            string refreshToken = RefreshTokenString.GetRefreshToken();
            user.RefreshToken = refreshToken;
            await UpdateUserWithRefreshToken(user);
            string accessToken;

            if (user.Role == "Admin")
            {
                accessToken = GenerateAdminJsonWebToken(secretKeyConfig, secretKeyDateTime);
                return CreateLoginResponse(0, "Admin", "Admin", "Unknown", "Unknown", accessToken, refreshToken);
            }
            else
            {
                accessToken = GenerateUserJsonWebToken(user, secretKeyConfig, secretKeyDateTime);
                return CreateLoginResponse(user.UserId, user.Name, user.Role, user.PhoneNumber, user.Address, accessToken, refreshToken);
            }
        }

        private async Task UpdateUserWithRefreshToken(User user)
        {
            _unitOfWork.Repository<User>().Update(user, user.UserId);
            await _unitOfWork.CommitAsync();
        }

        private UserLoginResponse CreateLoginResponse(int userId, string name, string role, string phone, string address, string accessToken, string refreshToken)
        {
            return new UserLoginResponse
            {
                UserId = userId,
                Name = name,
                Role = role,
                PhoneNumber = phone,
                Address = address,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string GenerateAdminJsonWebToken(string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "Admin"),
                new Claim("userId", "0"),
                new Claim(ClaimTypes.Role, "Admin"),
            };

            var token = new JwtSecurityToken(
                issuer: secretKey,
                audience: secretKey,
                claims: claims,
                expires: now.AddMinutes(86400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateUserJsonWebToken(User user, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.PhoneNumber),
                new Claim("userId", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, "User"),
            };

            var token = new JwtSecurityToken(
                issuer: secretKey,
                audience: secretKey,
                claims: claims,
                expires: now.AddMinutes(86400),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponse> UpdateCustomer(int customerId, CustomerRequest request)
        {
            try
            {
                User user = null;
                user = _unitOfWork.Repository<User>().Find(u => u.UserId == customerId);
                if (user == null)
                {
                    throw new CrudException(HttpStatusCode.NotFound, "Not found user with that id", customerId.ToString());
                }
                _mapper.Map<CustomerRequest, User>(request, user);
                await _unitOfWork.Repository<User>().Update(user, customerId);
                await _unitOfWork.CommitAsync();
                return _mapper.Map<User, UserResponse>(user);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update User Error", ex.Message);
            }
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest request)
        {
            if (!IsUniqueUser(request.PhoneNumber))
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Phone number already registered", request.PhoneNumber);
            }

            var check = CheckPhone(request.PhoneNumber);
            if (!check)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Phone number is not formatted", request.PhoneNumber);
            }

            var newUser = new User
            {
                PhoneNumber = request.PhoneNumber,
                Password = request.Password,
                Name = request.Name,
                Address = request.Address,
                Role = request.Role,
                Status = "True"
            };

            await _unitOfWork.Repository<User>().CreateAsync(newUser);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<UserResponse>(newUser);
        }

        public async Task<UserResponse> UpdateAsync(int id, UserRequest request)
        {
            try
            {
                var user = await _unitOfWork.Repository<User>().GetAsync(u => u.UserId == id);

                //if (user == null)
                //{
                //    throw new CrudException(HttpStatusCode.NotFound, "User not found", id.ToString());
                //}

                user.Name = request.Name;
                user.Password = request.Password;
                user.Address = request.Address;

                await _unitOfWork.CommitAsync();

                return _mapper.Map<UserResponse>(user);
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Update User Error", ex.Message);
            }
        }

        public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _unitOfWork.Repository<User>()
                    .Where(u => u.RefreshToken == refreshToken)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Invalid refresh token", "Refresh token is invalid.");
                }

                string newRefreshToken = RefreshTokenString.GetRefreshToken();
                string secretKeyConfig = _config["JWTSecretKey:SecretKey"];
                DateTime secretKeyDateTime = DateTime.UtcNow;
                string newAccessToken = GenerateUserJsonWebToken(user, secretKeyConfig, secretKeyDateTime);

                user.RefreshToken = newRefreshToken;
                await UpdateUserWithRefreshToken(user);

                return new RefreshTokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Refresh Token Error", ex.Message);
            }
        }

        public bool CheckPassword(User user, string password)
        {
            return user != null && user.Password == password;
        }
        public async Task<User> GetCustomerByPhone(string phone)
        {
            try
            {
                if (phone == null)
                {
                    throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Phone is invalid", "");
                }
                var user = await _unitOfWork.Repository<User>().GetAsync(u => u.PhoneNumber == phone);
                return user;
            }
            catch (CrudException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new CrudException(HttpStatusCode.BadRequest, "Get User By Phone Error", ex.InnerException?.Message);
            }
        }
    }
}
