﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.DataAccess.Models.Enums
{
    public partial class Enums
    {
        public partial class OrdersFilterEnums
        {
            public enum SortingDirection
            {
                LowToHigh = 1,
                HighToLow = 2
            }
        }
    }
}