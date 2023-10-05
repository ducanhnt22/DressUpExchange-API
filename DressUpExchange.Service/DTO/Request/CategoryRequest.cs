using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class CategoryRequest
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        [JsonIgnore]
        public string? Status { get; set; } = "Active";
    }
}
