using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int? ShopId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Category { get; set; }
        public bool? Availability { get; set; }
    }
}
