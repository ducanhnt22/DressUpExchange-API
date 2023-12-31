﻿using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class User
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            ProductFeedbacks = new HashSet<ProductFeedback>();
            Products = new HashSet<Product>();
            Vouchers = new HashSet<Voucher>();
        }

        public int UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public string? Status { get; set; }
        public string? RefreshToken { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
    }
}
