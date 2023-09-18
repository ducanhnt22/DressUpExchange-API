using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ProductFeedbackRequest
    {
        public int? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
    }
}
