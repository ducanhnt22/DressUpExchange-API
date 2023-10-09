using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Request
{
    public class UserRequest
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}
