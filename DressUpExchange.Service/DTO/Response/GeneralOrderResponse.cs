﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DressUpExchange.Service.DTO.Response
{
    public class GeneralOrderResponse
    {
        public int total { get; set; }

        public List<OrderResponse>?  orderResponses { get; set; }  = new List<OrderResponse>();

    }
}
