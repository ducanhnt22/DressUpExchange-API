using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductFeedbacks = new HashSet<ProductFeedback>();
        }

        public int ProductId { get; set; }
        public int? ShopId { get; set; }
        public string? Name { get; set; }
        public string? ProductImg { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public string? Category { get; set; }

        public virtual Shop? Shop { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
    }
}
