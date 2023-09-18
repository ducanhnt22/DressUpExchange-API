using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class OrderItemsRequest
    {
        public int? ProductId { get; set; }
        public int? UserSavedVoucherId { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
