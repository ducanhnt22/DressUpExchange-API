using AutoMapper;
using DressUpExchange.Data.Entity;
using DressUpExchange.Data.UnitOfWork;
using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Ultilities;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.Services
{

    public interface IFeedbackService
    {
        Task<bool> AddNewFeedback(int productID, FeedbackRequest feedbackRequest);

        Task<bool> UpdateNewFeedback(int feedBackID, FeedbackRequest feedbackRequest);

        Task<bool> DeleteFeedback(int feedBackID);

        Task<FeedbackResponse> GetFeedback(int feedbackId);
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
            var findProduct = await _unitOfWork.Repository<Product>().FindAsync(x => x.ProductId == productID);
            if (findProduct != null)
            {
                var newFeedback = _mapper.Map<ProductFeedback>(feedbackRequest);
                newFeedback.ProductId = productID;
                newFeedback.Status = "Active";

                await _unitOfWork.Repository<ProductFeedback>().CreateAsync(newFeedback);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteFeedback(int feedBackID)
        {
            ProductFeedback findFeedback = await _unitOfWork.Repository<ProductFeedback>().FindAsync(x => x.FeedbackId == feedBackID);
            if (findFeedback is not null)
            {
                findFeedback.Status = "Banned";

                await _unitOfWork.Repository<ProductFeedback>().Update(findFeedback, feedBackID);
                _unitOfWork.Commit();

                return true;

            }
            return false;
        }

        public async Task<FeedbackResponse> GetFeedback(int feedbackId)
        {
            List<FeedbackDetailResponse> feedbackDetailResponses = await QueryFormat.GetFeedbackResponseAsync(feedbackId);
          FeedbackResponse feedbackRequest = new FeedbackResponse()
          {
              total =  feedbackDetailResponses.Count,
              listFeedback = feedbackDetailResponses
          };

            return feedbackRequest;
        }

        public async Task<bool> UpdateNewFeedback(int feedBackID, FeedbackRequest feedbackRequest)
        {
            ProductFeedback productFeedback =  _unitOfWork.Repository<ProductFeedback>().Find(x => x.FeedbackId == feedBackID);
            if (productFeedback is not null)
            {
                productFeedback.UserId = feedbackRequest.UserID;
                productFeedback.Comment = feedbackRequest.Comment;
                productFeedback.Rating = feedbackRequest.Rating;
                await _unitOfWork.Repository<ProductFeedback>().Update(productFeedback, productFeedback.FeedbackId);
               await _unitOfWork.CommitAsync();
                return true;

            }
            return false;
        }
    }
}
