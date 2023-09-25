using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class FeedbackDetailResponse
    {
        public int feedBackId { get; set; }

        public string? userName { get; set; }

        public string? comment { get; set; }

        public int  rating { get; set; }
    }
}
