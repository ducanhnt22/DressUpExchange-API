using AutoMapper;
using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using Firebase.Auth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Twilio.Rest.Api.V2010.Account;

namespace DressUpExchange.Service.Ultilities
{
    public static class QueryFormat
    {
        private static DressUpExchanceContext _context = new DressUpExchanceContext();






        public static async Task<List<OrderResponse>> getAllOrder(this int page, int pageSize)
        {
            var response = await (from order in _context.Orders
                                  join orderitem in _context.OrderItems on order.OrderId equals orderitem.OrderId
                                  join product in _context.Products on orderitem.ProductId equals product.ProductId
                                  select new GeneralOrderResponse
                                  {
                                      total = (int)order.TotalAmount,
                                      orderResponses = (from or in _context.Orders
                                                        join oi in _context.OrderItems on or.OrderId equals oi.OrderId
                                                        select new OrderResponse
                                                        {
                                                            orderDate = (DateTime)or.OrderDate,
                                                            totalAmount = (int)order.TotalAmount,
                                                            orderItems = (from or1 in _context.OrderItems
                                                                          join n in _context.Products on or1.ProductId equals n.ProductId
                                                                          where n.ProductId == or1.ProductId
                                                                          select new OrderItemResponse
                                                                          {
                                                                              ProductID = product.ProductId,
                                                                              ProductName = product.Name,
                                                                              quantityBuy = (int)or1.Quantity,
                                                                              status = or1.Status
                                                                          }
                                                                          ).ToList()


                                                        }).ToList()



                                  }).FirstOrDefaultAsync();




            var result = (from order in _context.Orders
                          join orderItems in _context.OrderItems on order.OrderId equals orderItems.OrderId
                          select new OrderResponse
                          {
                              orderDate = (DateTime)order.OrderDate,
                              totalAmount = (int)order.TotalAmount,
                              orderItems = (from ots in _context.OrderItems
                                            join product in _context.Products on ots.ProductId equals product.ProductId
                                            select new OrderItemResponse
                                            {
                                                ProductID = product.ProductId,
                                                ProductName = product.Name,
                                                quantityBuy = (int)product.Quantity,
                                                status = ots.Status
                                            }).ToList()
                          }).ToList();
                
            return result;

        }
        public static async Task<GeneralOrderResponse> getOrder(this int customerId, string status, int page, int pageSize)
        {
            var response = await (from order in _context.Orders
                                  join orderitem in _context.OrderItems on order.OrderId equals orderitem.OrderId
                                  join product in _context.Products on orderitem.ProductId equals product.ProductId
                                  where order.UserId == customerId && order.Status == status
                                  select new GeneralOrderResponse
                                  {
                                      total = (int)order.TotalAmount,
                                      orderResponses = (from or in _context.Orders
                                                        join oi in _context.OrderItems on or.OrderId equals oi.OrderId
                                                        where oi.OrderId == order.OrderId
                                                        select new OrderResponse
                                                        {
                                                            orderDate = (DateTime)or.OrderDate,
                                                            totalAmount = (int)order.TotalAmount,
                                                            orderItems = (from or1 in _context.OrderItems
                                                                          join n in _context.Products on or1.ProductId equals n.ProductId
                                                                          where n.ProductId == or1.ProductId
                                                                          select new OrderItemResponse
                                                                          {
                                                                              ProductID = product.ProductId,
                                                                              ProductName = product.Name,
                                                                              quantityBuy = (int)or1.Quantity,
                                                                              status = or1.Status
                                                                          }
                                                                          ).ToList()


                                                        }).Skip(page).Take(pageSize).ToList()



                                  }).FirstOrDefaultAsync();





            return response;


        }

        public static async Task<List<FeedbackDetailResponse>> GetFeedbackResponseAsync(this int productID, int page, int pageSize)
        {

            var response = (from productfeedback in _context.ProductFeedbacks
                            join user in _context.Users on productfeedback.UserId equals user.UserId
                            where productfeedback.ProductId == productID
                            select new FeedbackDetailResponse
                            {
                                comment = productfeedback.Comment,
                                feedBackId = productfeedback.FeedbackId,
                                rating = (int)productfeedback.Rating,
                                userName = user.Name

                            }).Skip(page).Take(pageSize).ToList();
            return response;

        }


        public static async Task<IQueryable<ProductResponse>> GetProductResponsesAsync(ProductGetRequest request)
        {


            var result = _context.Products
                         .Include(p => p.ProductImages) // Eagerly load images
                         .Include(p => p.User)
                         .Where(p => p.Status == "Active")
                         .Select(product => new ProductResponse
                         {
                             UserId = product.UserId,
                             Description = product.Description,
                             Name = product.Name,
                             Price = product.Price,
                             Quantity = product.Quantity,
                             Thumbnail = product.Thumbnail,
                             Size = product.Size,
                             ProductId = product.ProductId,
                             Images = product.ProductImages.Select(pi => pi.ImageUrl).ToList(),
                             User = new UserResponse
                             {
                                 //UserId = product.User.UserId,
                                 Phone = product.User.PhoneNumber,
                                 Name = product.User.Name,
                                 Address = product.User.Address,
                                 Role = product.User.Role
                             }
                         })
                         .ToList().AsQueryable();

            return result;
        }
    }
}