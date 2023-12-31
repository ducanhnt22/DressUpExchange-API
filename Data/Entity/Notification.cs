﻿using System;
using System.Collections.Generic;

namespace DressUpExchange.Data.Entity
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public DateTime? Timestamp { get; set; }
        public string? Status { get; set; }

        public virtual User? User { get; set; }
    }
}
