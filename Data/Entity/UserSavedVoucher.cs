using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class UserSavedVoucher
    {
        public UserSavedVoucher()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int UserSavedVoucherId { get; set; }
        public int? UserId { get; set; }
        public int? VoucherId { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
