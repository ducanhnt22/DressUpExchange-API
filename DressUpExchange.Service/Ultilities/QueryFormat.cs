using AutoMapper;
using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Helpers;
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
        private static DressupExchanceContext _context = new DressupExchanceContext();

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
                                                            OrderId = order.OrderId,
                                                            orderDate = (DateTime)or.OrderDate,
                                                            totalAmount = (int)order.TotalAmount,
                                                            Status = order.Status,
                                                            orderItems = (from or1 in _context.OrderItems
                                                                          join n in _context.Products on or1.ProductId equals n.ProductId
                                                                          where n.ProductId == or1.ProductId
                                                                          select new OrderItemResponse
                                                                          {
                                                                              ProductID = product.ProductId,
                                                                              ProductName = product.Name,
                                                                              BuyingQuantity = (int)or1.Quantity,
                                                                              status = or1.Status,
                                                                              price = or1.Price
                                                                          }
                                                                          ).ToList()

                                                        }).ToList()

                                  }).FirstOrDefaultAsync();


            var result = (from order in _context.Orders
                          join orderItems in _context.OrderItems on order.OrderId equals orderItems.OrderId
                          select new OrderResponse
                          {
                              OrderId = order.OrderId,
                              orderDate = (DateTime)order.OrderDate,
                              totalAmount = (int)order.TotalAmount,
                              Status = order.Status,
                              orderItems = (from ots in _context.OrderItems
                                            join product in _context.Products on ots.ProductId equals product.ProductId
                                            select new OrderItemResponse
                                            {
                                                ProductID = product.ProductId,
                                                ProductName = product.Name,
                                                BuyingQuantity = (int)product.Quantity,
                                                status = ots.Status,
                                                price = ots.Price
                                            }).ToList()
                          }).ToList();

            var result1 = (from order in _context.Orders
                           select new OrderResponse
                           {
                               OrderId = order.OrderId,
                               orderDate = (DateTime)order.OrderDate,
                               totalAmount = (float)order.TotalAmount,
                               Status = order.Status,
                               orderItems = (from orderItemSp in _context.OrderItems
                                             join product in _context.Products on orderItemSp.ProductId equals product.ProductId
                                             where order.OrderId == orderItemSp.OrderId
                                             select new OrderItemResponse
                                             {
                                                 ProductID = product.ProductId,
                                                 ProductName = product.Name,
                                                 BuyingQuantity = orderItemSp.Quantity,
                                                 price = orderItemSp.Price,
                                                 status = orderItemSp.Status,
                                                 Laundry = (from orderItemSp2 in _context.OrderItems
                                                            join laundry in _context.Laundries on orderItemSp2.LaundryId equals laundry.LaundryId
                                                            where orderItemSp2.OrderItemId == orderItemSp.OrderItemId
                                                            select new LaundryResponse
                                                            {
                                                                LaundryName = laundry.LaundryName,
                                                                Price = laundry.Price
                                                            }).ToList()

                                             }).ToList()
                           }).ToList();

            return result1;

        }
        public static  async Task<List<OrderResponse>> GetOrders(int customerId, string status, int page, int pageSize)
        {
            /*  var response =  await (from order in _context.Orders
                                    join orderitem in _context.OrderItems on order.OrderId equals orderitem.OrderId
                                    where order.UserId == customerId
                                     select new GeneralOrderResponse
                                    {
                                        total = (int)order.TotalAmount,
                                        orderResponses = (from or in _context.Orders
                                                          join oi in _context.OrderItems on or.OrderId equals oi.OrderId
                                                          join fl in _context.Laundries on oi.LaundryId equals fl.LaundryId
                                                          where  or.UserId == customerId && or.Status == status
                                                          select new OrderResponse
                                                          {
                                                              orderDate = (DateTime)or.OrderDate,
                                                              totalAmount = (int)or.TotalAmount,
                                                              orderItems = (from or1 in _context.OrderItems
                                                                            join n in _context.Products on or1.ProductId equals n.ProductId
                                                                            select new OrderItemResponse
                                                                            {
                                                                                ProductID = or1.ProductId,
                                                                                ProductName = n.Name,
                                                                                quantityBuy = (int)or1.Quantity,
                                                                                status = or1.Status,
                                                                                Laundry = (from oi2 in _context.OrderItems
                                                                                           join l in _context.Laundries on oi2.LaundryId equals l.LaundryId
                                                                                           select new LaundryResponse
                                                                                           {
                                                                                               LaundryName = l.LaundryName,
                                                                                               Price = l.Price
                                                                                           }).ToList()
                                                                            }
                                                              ).ToList()
                                                          }
                                        ).Skip(page).Take(pageSize).ToList()
                                    }
                              ).FirstOrDefaultAsync();*/
            var result1 = (from order in _context.Orders
                           where order.UserId == customerId
                           select new OrderResponse
                           {
                               OrderId = order.OrderId,
                               orderDate = (DateTime)order.OrderDate,
                               totalAmount = (float)order.TotalAmount,
                               Status = order.Status,
                               orderItems = (from orderItemSp in _context.OrderItems
                                             join product in _context.Products on orderItemSp.ProductId equals product.ProductId
                                             where order.OrderId == orderItemSp.OrderId
                                             select new OrderItemResponse
                                             {
                                                 ProductID = product.ProductId,
                                                 ProductName = product.Name,
                                                 BuyingQuantity = orderItemSp.Quantity,
                                                 price = orderItemSp.Price,
                                                 status = orderItemSp.Status,
                                                 Laundry = (from orderItemSp2 in _context.OrderItems
                                                            join laundry in _context.Laundries on orderItemSp2.LaundryId equals laundry.LaundryId
                                                            where orderItemSp2.OrderItemId == orderItemSp.OrderItemId
                                                            select new LaundryResponse
                                                            {
                                                                LaundryName = laundry.LaundryName,
                                                                Price = laundry.Price
                                                            }).ToList()

                                             }).ToList()
                           }).ToList();

            return result1;
        }
        public static async Task<List<OrderResponse>> GetOrdersByOrderId(int OrderId, string status, int page, int pageSize)
        {
            /*  var response =  await (from order in _context.Orders
                                    join orderitem in _context.OrderItems on order.OrderId equals orderitem.OrderId
                                    where order.UserId == customerId
                                     select new GeneralOrderResponse
                                    {
                                        total = (int)order.TotalAmount,
                                        orderResponses = (from or in _context.Orders
                                                          join oi in _context.OrderItems on or.OrderId equals oi.OrderId
                                                          join fl in _context.Laundries on oi.LaundryId equals fl.LaundryId
                                                          where  or.UserId == customerId && or.Status == status
                                                          select new OrderResponse
                                                          {
                                                              orderDate = (DateTime)or.OrderDate,
                                                              totalAmount = (int)or.TotalAmount,
                                                              orderItems = (from or1 in _context.OrderItems
                                                                            join n in _context.Products on or1.ProductId equals n.ProductId
                                                                            select new OrderItemResponse
                                                                            {
                                                                                ProductID = or1.ProductId,
                                                                                ProductName = n.Name,
                                                                                quantityBuy = (int)or1.Quantity,
                                                                                status = or1.Status,
                                                                                Laundry = (from oi2 in _context.OrderItems
                                                                                           join l in _context.Laundries on oi2.LaundryId equals l.LaundryId
                                                                                           select new LaundryResponse
                                                                                           {
                                                                                               LaundryName = l.LaundryName,
                                                                                               Price = l.Price
                                                                                           }).ToList()
                                                                            }
                                                              ).ToList()
                                                          }
                                        ).Skip(page).Take(pageSize).ToList()
                                    }
                              ).FirstOrDefaultAsync();*/
            var result1 = (from order in _context.Orders
                           where order.OrderId == OrderId
                           select new OrderResponse
                           {
                               OrderId = order.OrderId,
                               orderDate = (DateTime)order.OrderDate,
                               totalAmount = (float)order.TotalAmount,
                               Status = order.Status,
                               orderItems = (from orderItemSp in _context.OrderItems
                                             join product in _context.Products on orderItemSp.ProductId equals product.ProductId
                                             where order.OrderId == orderItemSp.OrderId
                                             select new OrderItemResponse
                                             {
                                                 ProductID = product.ProductId,
                                                 ProductName = product.Name,
                                                 BuyingQuantity = orderItemSp.Quantity,
                                                 price = orderItemSp.Price,
                                                 status = orderItemSp.Status,
                                                 Laundry = (from orderItemSp2 in _context.OrderItems
                                                            join laundry in _context.Laundries on orderItemSp2.LaundryId equals laundry.LaundryId
                                                            where orderItemSp2.OrderItemId == orderItemSp.OrderItemId
                                                            select new LaundryResponse
                                                            {
                                                                LaundryName = laundry.LaundryName,
                                                                Price = laundry.Price
                                                            }).ToList()

                                             }).ToList()
                           }).ToList();

            return result1;
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
            var query = _context.Products
                                .Include(p => p.ProductImages)
                                .Include(p => p.User)
                                .Where(p => p.Status == "Active");

            if (request.CategoryId != null)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId);
            }

            if (request.SortOrderPrice != null)
            {
                if (request.SortOrderPrice == SortOrder.Ascending)
                {
                    query = query.OrderBy(p => p.Price);
                }
                if (request.SortOrderPrice == SortOrder.Descending)
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }
            if (request.SortOrderProductId != null)
            {
                if (request.SortOrderProductId == SortOrder.Ascending)
                {
                    query = query.OrderBy(p => p.ProductId);
                }
                if (request.SortOrderProductId == SortOrder.Descending)
                {
                    query = query.OrderByDescending(p => p.ProductId);
                }
            }

            var result = query
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