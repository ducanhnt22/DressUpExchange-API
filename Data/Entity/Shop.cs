using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Shop
    {
        public int ShopId { get; set; }
        public int? UserId { get; set; }
        public string? ShopName { get; set; }
    }
}
