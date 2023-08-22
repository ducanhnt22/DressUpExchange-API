using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class User
    {
        public int UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public int? ShopId { get; set; }
    }
}
