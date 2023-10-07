using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public  class ProductResponseGeneral
    {
        public int Count { get; set; }

        public List<ProductResponse>? Items { get; set; }

    }
}
