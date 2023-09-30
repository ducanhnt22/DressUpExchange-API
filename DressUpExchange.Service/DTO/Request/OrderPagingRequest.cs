using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class OrderPagingRequest : PagingRequest
    {
        public  string? Status { get; set; }
    }
}
