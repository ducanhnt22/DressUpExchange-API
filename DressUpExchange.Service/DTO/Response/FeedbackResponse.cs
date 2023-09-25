using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class FeedbackResponse
    {
        public int total { get; set; }

        public List<FeedbackDetailResponse>? listFeedback { get; set; }
    }
}
