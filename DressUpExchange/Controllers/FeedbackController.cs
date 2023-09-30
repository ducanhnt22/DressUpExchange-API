using DressUpExchange.Service.DTO.Request;
using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.DTO.State;
using DressUpExchange.Service.Services;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = RoleNames.Customer)]
        [HttpPost]
        public async Task<ActionResult> CreateFeedback(int id, FeedbackRequest feedbackRequest)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool check = await _feedbackService.AddNewFeedback(id, feedbackRequest);

            return Ok("Create Feedback Sucessfully");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpPut]
        public async Task<ActionResult> UpdateFeedback(int id, FeedbackRequest feedbackRequest)
        {

            await _feedbackService.UpdateNewFeedback(id, feedbackRequest);
            return Ok("Update Feedback Sucessfully");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpDelete]
        public async Task<ActionResult> DeleteFeedback(int id)
        {
            await _feedbackService.DeleteFeedback(id);
            return Ok("Delete Feedback Sucessfully");
        }

        [Authorize(Roles = RoleNames.Customer)]
        [HttpGet]
        public async Task<ActionResult<FeedbackResponse>> GetAction(int id, [FromQuery] PagingRequest pagingRequest)
        {
            FeedbackResponse feedbackResponse = await _feedbackService.GetFeedback(id, pagingRequest);
            return Ok(feedbackResponse);
        }
    }
}
