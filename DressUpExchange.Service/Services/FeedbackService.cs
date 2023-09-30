using AutoMapper;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Exceptions;
using DressUpExchange.Service.Ultilities;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{

    public interface IFeedbackService
    {
        Task<bool> AddNewFeedback(int productID, FeedbackRequest feedbackRequest);

        Task<bool> UpdateNewFeedback(int feedBackID, FeedbackRequest feedbackRequest);

        Task<bool> DeleteFeedback(int feedBackID);

        Task<FeedbackResponse> GetFeedback(int feedbackId, PagingRequest pagingRequest);
    }
    public class FeedbackService : IFeedbackService
    {

        private static IUnitOfWork _unitOfWork;
        private static IMapper _mapper;
        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> AddNewFeedback(int productID, FeedbackRequest feedbackRequest)
        {
            try
            {
                var findProduct = await _unitOfWork.Repository<Product>().FindAsync(x => x.ProductId == productID);
                if (findProduct is null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Product Not Found!!!", "");
                }
                if ( await CheckProductInOrder(productID, feedbackRequest.UserID) is false)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "User didn't buy this product !!!", "");
                }

                var newFeedback = _mapper.Map<ProductFeedback>(feedbackRequest);
                newFeedback.ProductId = productID;
                newFeedback.Status = "Active";
                await _unitOfWork.Repository<ProductFeedback>().CreateAsync(newFeedback);
                _unitOfWork.Commit();
                return true;

            }
            catch (Exception ex)
            {

                throw new CrudException(HttpStatusCode.BadRequest, "Insert Product Error!!!", ex.InnerException?.Message);
            }

        }

        public async Task<bool> DeleteFeedback(int feedBackID)
        {
            try
            {
                ProductFeedback findFeedback = await _unitOfWork.Repository<ProductFeedback>().FindAsync(x => x.FeedbackId == feedBackID);
                if (findFeedback is null)
                {
                    throw new CrudException(HttpStatusCode.BadRequest, "Feedback Not Found!!!", "");

                }
                findFeedback.Status = "Banned";

                await _unitOfWork.Repository<ProductFeedback>().Update(findFeedback, feedBackID);
                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {

                throw new CrudException(HttpStatusCode.BadRequest, "Delete Feedback Error!!!", ex.InnerException?.Message);
            }

        }

        public async Task<FeedbackResponse> GetFeedback(int feedbackId,PagingRequest pagingRequest)
        {
            try
            {
                List<FeedbackDetailResponse> feedbackDetailResponses = await QueryFormat.GetFeedbackResponseAsync(feedbackId,pagingRequest.Page,pagingRequest.PageSize);
                FeedbackResponse feedbackRequest = new FeedbackResponse()
                {
                    total = feedbackDetailResponses.Count,
                    listFeedback = feedbackDetailResponses
                };

                return feedbackRequest;
            }
            catch (Exception ex)
            {

                throw new CrudException(HttpStatusCode.BadRequest, "Get Feeback Error !!!", ex.InnerException?.Message);
            }

        }

        public async Task<bool> UpdateNewFeedback(int feedBackID, FeedbackRequest feedbackRequest)
        {
            try
            {
                ProductFeedback productFeedback = _unitOfWork.Repository<ProductFeedback>().Find(x => x.FeedbackId == feedBackID);
                if (productFeedback is null)
                {

                    throw new CrudException(HttpStatusCode.BadRequest, "Product Feedback is not found", "");


                }
                productFeedback.UserId = feedbackRequest.UserID;
                productFeedback.Comment = feedbackRequest.Comment;
                productFeedback.Rating = feedbackRequest.Rating;
                await _unitOfWork.Repository<ProductFeedback>().Update(productFeedback, productFeedback.FeedbackId);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw new CrudException(HttpStatusCode.BadRequest, "Update Feedback have Error", ex.InnerException?.Message);
            }

        }

        public async Task<bool> CheckProductInOrder(int productID, int userId)
        {
            int checkOrderId = _unitOfWork.Repository<Order>().Where(x => x.UserId == userId).FirstOrDefault()?.OrderId ?? 0;
            int checkOrderId2 = _unitOfWork.Repository<OrderItem>().Where(x => x.OrderId == productID).FirstOrDefault()?.OrderId ?? 0;
            return checkOrderId == checkOrderId2;

        }
    }
}
