using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class ChangePasswordRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
    }
}
