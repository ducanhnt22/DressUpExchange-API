using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Laundry
    {
        public Laundry()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int LaundryId { get; set; }
        public string? LaundryName { get; set; }
        public string? Price { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
