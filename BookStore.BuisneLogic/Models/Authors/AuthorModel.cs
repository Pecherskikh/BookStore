﻿using BookStore.BusinessLogic.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogic.Models.Authors
{
    public class AuthorModel : BaseModel
    {
        public ICollection<AuthorModelItem> Items = new List<AuthorModelItem>();
    }
}