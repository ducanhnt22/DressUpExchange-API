using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class UserResponse
    {
        public int UserId { get; internal set; }
        public string? Name { get; internal set; }
        public string? Role { get; internal set; }
    }
}
