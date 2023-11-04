using DressUpExchange.Service.DTO.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class UpdateOrderRequest
    {
        [JsonIgnore]
        public int OrderId { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        [JsonIgnore]
        public DateTime? OrderDate { get; set; } = DateTime.UtcNow.AddHours(7);
        [JsonIgnore]
        public decimal? TotalAmount { get; set; }
        [JsonIgnore]
        public string? ShippingAddress { get; set; }
        public string? Status { get; set; }
    }
}
