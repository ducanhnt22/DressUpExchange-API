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
            Vouchers = new HashSet<Voucher>();
        }

        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Status { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryId { get; set; }
        public string? Thumbnail { get; set; }
        public string? Size { get; set; }
        public string? ImagesUrl { get; set; }
        public virtual Category? Category { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
