using DressUpExchange.Service.DTO.Response;
using DressUpExchange.Service.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ProductGetRequest
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public SortOrder? SortOrder { get; set; }
        public string? Status { get; set; } = "Active";
        public int? Quantity { get; set; }
        public string? Thumbnail { get; set; }
        public string? Size { get; set; }
        public int? CategoryId { get; set; }
        [JsonIgnore]
        public virtual UserResponse? User { get; set; }
    }
}
