using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class UserResponse
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string? Phone { get; internal set; }
        public string? Name { get; internal set; }
        public string? Role { get; internal set; }
        public string? Address { get; internal set; }
    }
}
