using DressUpExchange.Service.DTO.State;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class OrderItemsRequest
    {
        public int? ProductId { get; set; }
        public int? VoucherId { get; set; } 
        public int? LaundryId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = OrderState.Processing.ToString();
    }
}
