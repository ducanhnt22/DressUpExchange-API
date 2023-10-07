using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ProductRequest
    {
        [JsonIgnore]
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Size { get; set; }
        public decimal? Price { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
        public int? Quantity { get; set; }
        public string? Thumbnail { get; set; }
        public virtual ICollection<string>? Images { get; set; }
        public int? CategoryId { get; set; }
    }
}
