using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? VoucherId { get; set; }
        public int? Quantity { get; set; }
        public string? Status { get; set; }
        public string? Price { get; set; }
        public int? LaundryId { get; set; }

        public virtual Laundry? Laundry { get; set; }
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Voucher? Voucher { get; set; }
    }
}
