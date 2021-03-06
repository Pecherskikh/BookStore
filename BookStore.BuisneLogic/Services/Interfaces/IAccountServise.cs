﻿using BookStore.BusinessLogic.Models.Base;
using BookStore.BusinessLogic.Models.Users;
using BookStore.DataAccess.Entities;
using BookStore.DataAccess.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusinessLogic.Services.Interfaces
{
    public interface IAccountServise
    {
        Task<UserModelItem> FindByIdAsync(string userId);
        Task<UserModelItem> FindByEmailAsync(string email);
        Task<UserModelItem> FindByNameAsync(string userName);
        Task<BaseModel> CreateAsync(ApplicationUser user);
        Task RemoveAsync(ApplicationUser user);
        Task<BaseModel> Register();
        Task<BaseModel> ConfirmEmail(string userId, string token);
        Task<BaseModel> ForgotPassword(string userEmail);
        Task<bool> CheckUserAsync(ApplicationUser user, string password, bool lockoutOnFailure);
        Task SignInAsync(ApplicationUser user, bool isPersistent);
    }
}
