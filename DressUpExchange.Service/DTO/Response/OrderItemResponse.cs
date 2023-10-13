using DressUpExchange.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class OrderItemResponse
    {
        public int? ProductID { get; set; }
        public string? ProductName { get; set; }
        public int? quantityBuy { get; set; }
        public string? status { get;set; }
        public string? price { get;set; }
        public List<LaundryResponse> Laundry { get; set; }
    }
}
