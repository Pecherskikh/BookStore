﻿using BookStore.BusinessLogic.Models.Orders;

namespace BookStore.BusinessLogic.Extensions.OrderExtensions
{
    public static class OrderModelItemDAMapToOrderModelItemBL
    {
        public static OrderModelItem Map(this BookStore.DataAccess.Models.Orders.OrderModelItem order)
        {
            return new OrderModelItem()
            {
                Id = order.Id,
                Date = order.Date,
                UserName = order.UserName,
                UserEmail = order.UserEmail,
                ProductType = order.Product,
                Title = order.Title,
                Quantity = order.Quantity,
                OrderAmount = order.OrderAmount
            };
        }
    }
}