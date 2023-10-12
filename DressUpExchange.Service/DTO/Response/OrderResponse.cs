﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class OrderResponse
    {
        public float totalAmount { get; set; }

        public DateTime orderDate { get; set; }

        public List<OrderItemResponse>? orderItems { get; set; }
    }
}
