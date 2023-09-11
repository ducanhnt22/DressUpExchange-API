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
        public int? UserId { get; set; }
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public decimal? DiscountAmount { get; set; }
        public int? RemainingCount { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? Status { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<UserSavedVoucher> UserSavedVouchers { get; set; }
    }
}
