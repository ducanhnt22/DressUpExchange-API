using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Voucher
    {
        public Voucher()
        {
            UserSavedVouchers = new HashSet<UserSavedVoucher>();
        }

        public int VoucherId { get; set; }
        public int? ShopId { get; set; }
        public string? Code { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? RemainingCount { get; set; }

        public virtual Shop? Shop { get; set; }
        public virtual ICollection<UserSavedVoucher> UserSavedVouchers { get; set; }
    }
}
