using System;
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
            Shops = new HashSet<Shop>();
            UserSavedVouchers = new HashSet<UserSavedVoucher>();
        }

        public int UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Role { get; set; }
        public int? ShopId { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<UserSavedVoucher> UserSavedVouchers { get; set; }
    }
}
