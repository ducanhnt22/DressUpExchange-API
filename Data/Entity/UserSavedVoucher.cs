using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class UserSavedVoucher
    {
        public int UserSavedVoucherId { get; set; }
        public int? UserId { get; set; }
        public int? VoucherId { get; set; }

        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
    }
}
