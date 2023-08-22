using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Shop
    {
        public Shop()
        {
            Products = new HashSet<Product>();
            Vouchers = new HashSet<Voucher>();
        }

        public int ShopId { get; set; }
        public int? UserId { get; set; }
        public string? ShopLogo { get; set; }
        public string? ShopName { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
