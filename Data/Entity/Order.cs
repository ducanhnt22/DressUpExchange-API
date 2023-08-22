using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
