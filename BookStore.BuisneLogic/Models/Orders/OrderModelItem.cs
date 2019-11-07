﻿using BookStore.BusinessLogic.Models.Base;
using System;
using static BookStore.BusinessLogic.Models.Enums.Enums;

namespace BookStore.BusinessLogic.Models.Orders
{
    public class OrderModelItem : BaseModel
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public long PaymentId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public TypePrintingEdition ProductType { get; set; }
        public string Title { get; set; }
        public long Quantity { get; set; }
        public long OrderAmount { get; set; }
    }
}
