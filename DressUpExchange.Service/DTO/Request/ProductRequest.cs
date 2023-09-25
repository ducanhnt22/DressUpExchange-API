using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
        public int? Quantity { get; set; }
        public string? Thumbnail { get; set; }
        public virtual ICollection<ProductImageRequest>? Images { get; set; }
        public int CategoryId { get; set; }
    }
}
