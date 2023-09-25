using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace DressUpExchange.API.Controllers
{
    [ApiController]
    [Route("/api/feedbacks")]
    public class FeedbackController : ControllerBase
    {

        private static IFeedbackService _feedbackService;
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpPost]
        public async Task<ActionResult> CreateFeedback(int id, FeedbackRequest feedbackRequest)
        {
            await _feedbackService.AddNewFeedback(id, feedbackRequest);
            return Ok("Create Feedback Sucessfully");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateFeedback(int id, FeedbackRequest feedbackRequest)
        {

            await _feedbackService.UpdateNewFeedback(id, feedbackRequest);
            return Ok("Update Feedback Sucessfully");
        }


        [HttpDelete]
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            await _feedbackService.DeleteFeedback(id);
            return Ok("Delete Feedback Sucessfully");
        }


        [HttpGet]
        public async Task<ActionResult<FeedbackResponse>> GetAction(int id)
        {
            FeedbackResponse feedbackResponse = await _feedbackService.GetFeedback(id);
            return Ok(feedbackResponse);
        }
    }
}
