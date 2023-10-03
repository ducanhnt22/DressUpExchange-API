using DressUpExchange.Service.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ProductGetRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        //public string? Status { get; set; }
        public int? Quantity { get; set; }
        public string? Thumbnail { get; set; }
        public string? Size { get; set; }
        public int? CategoryId { get; set; }
    }
}
