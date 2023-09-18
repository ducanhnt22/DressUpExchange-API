using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class RegisterRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string? Role { get; set; } = "User";
    }
}
