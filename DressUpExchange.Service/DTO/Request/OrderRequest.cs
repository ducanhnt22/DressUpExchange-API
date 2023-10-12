using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class OrderRequest
    {
        [JsonIgnore]
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = OrderState.Processing.ToString();
        public virtual ICollection<OrderItemsRequest>? OrderItemsRequest { get; set; }
    }
}
