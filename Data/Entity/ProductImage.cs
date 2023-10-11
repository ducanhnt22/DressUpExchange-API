using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class ProductImage
    {
        public int ImageId { get; set; }
        public int? ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Status { get; set; }
        public virtual Product? Product { get; set; }
    }
}
