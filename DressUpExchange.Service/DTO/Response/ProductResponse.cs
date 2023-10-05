using DressUpExchange.Data.Entity;
using DressUpExchange.Service.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
        public string? Size { get; set; }
        public string? Thumbnail { get; set; }
        public virtual ICollection<ProductImageRequest> Images { get; set; }
        public virtual CategoryRequest? Category { get; set; }
    }
}
