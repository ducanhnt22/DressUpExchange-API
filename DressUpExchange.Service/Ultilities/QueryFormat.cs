using AutoMapper;
using AutoMapper.QueryableExtensions;
using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Ultilities
{
    public static class QueryFormat
    {
         private static DressUpExchanceContext _context = new DressUpExchanceContext();
       
      

        public static  async Task<GeneralOrderResponse> getOrder(this int customerId,string status,int page,int pageSize)
        {
            var response =  await (from order in _context.Orders
                           join orderitem in _context.OrderItems on order.OrderId equals orderitem.OrderId
                           join product in _context.Products on orderitem.ProductId equals product.ProductId
                           where order.UserId == customerId && order.Status == status
                           select new GeneralOrderResponse
                           {
                               total = (int)order.TotalAmount,
                               orderResponses = (from or in _context.Orders 
                                                 join oi in _context.OrderItems on or.OrderId equals oi.OrderId
                                                 where  oi.OrderId == order.OrderId
                                                 select new OrderResponse
                                                 {
                                                     orderDate = (DateTime)or.OrderDate,
                                                     totalAmount = (int)order.TotalAmount,
                                                     orderItems = (from or1 in _context.OrderItems
                                                                   join  n in _context.Products on or1.ProductId equals n.ProductId
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

        public static async Task<List<FeedbackDetailResponse>> GetFeedbackResponseAsync(this int productID,int page, int pageSize)
        {
            var response =  (from productfeedback  in _context.ProductFeedbacks
                            join user in _context.Users on productfeedback.UserId equals user.UserId    
                            where productfeedback.ProductId == productID
                             select new FeedbackDetailResponse
                             {
                                 comment = productfeedback.Comment,
                                 feedBackId =  productfeedback.FeedbackId,
                                 rating = (int)productfeedback.Rating,
                                 userName = user.Name

                             }).Skip(page).Take(pageSize).ToList();
            return response;
        
        }

    }
}
