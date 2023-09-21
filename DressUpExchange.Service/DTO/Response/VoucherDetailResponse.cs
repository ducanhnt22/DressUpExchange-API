using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class VoucherDetailResponse
    {
        public Guid voucherId { get; set; }

        public string? voucherName { get; set;}

        public string? voucherCode { get; set;}

        public int discountAmount { get; set;}

        public DateTime expireDate { get; set;}

    }
}
