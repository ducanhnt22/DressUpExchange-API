using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class UpdateVoucherRequest
    {
        public string? newName { get; set; }
    
        public string? newCode { get; set; }

        public string? newDismountAmount { get; set; }

        public int newTotal { get;set; }

        public DateTime newExpireDate { get; set; }
    }
}
