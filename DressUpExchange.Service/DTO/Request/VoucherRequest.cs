using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class VoucherRequest
    {
        [JsonIgnore]
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? RemainingCount { get; set; }
        public DateTime? ExpireDate { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";

    }
}
