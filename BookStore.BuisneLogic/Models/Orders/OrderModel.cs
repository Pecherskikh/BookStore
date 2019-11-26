﻿using BookStore.BusinessLogic.Models.Base;
using System.Collections.Generic;

namespace BookStore.BusinessLogic.Models.Orders
{
    public class OrderModel : BaseModel
    {
        public long PageAmount { get; set; }
        public ICollection<OrderModelItem> Items { get; set; } = new List<OrderModelItem>();
    }
}
