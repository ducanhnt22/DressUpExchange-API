using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class VoucherResponse
    {
         public int? total { get; set; }

        public  List<VoucherDetailResponse> vouchers { get; set; }

       
    }
}
