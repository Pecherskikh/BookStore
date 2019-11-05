﻿using System.Threading.Tasks;
using BookStore.DataAccess.Repositories.Interfaces;
using BookStore.BusinessLogic.Services.Interfaces;
using BookStore.DataAccess.Entities;
using BookStore.BusinessLogic.Helpers.Interfaces;
using BookStore.BusinessLogic.Models.Base;
using BookStore.BusinessLogic.Common.Constants;
using BookStore.BusinessLogic.Models.Users;
using BookStore.BusinessLogic.Extensions.UserExtensions;
using BookStore.BusinessLogic.Extensions;

namespace BookStore.BusinessLogic.Services
{
    public class AccountService : IAccountServise
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly ICreatePasswordHelper _createPasswordHelper;

        public AccountService(IUserRepository userRepository, IEmailHelper emailHelper, ICreatePasswordHelper createPasswordHelper)
        {
            _userRepository = userRepository;
            _emailHelper = emailHelper;
            _createPasswordHelper = createPasswordHelper;
        }

        public async Task<UserModelItem> FindByIdAsync(string userId)
        {
            var resultModel = new UserModelItem();
            if (string.IsNullOrWhiteSpace(userId))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserIdIsEmptyError);
                return resultModel;
            }
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotFoundError);
                return resultModel;
            }
            return user.Map();
        }

        public async Task<UserModelItem> FindByEmailAsync(string email)
        {
            var resultModel = new UserModelItem();
            if (string.IsNullOrWhiteSpace(email))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserEmailIsEmptyError);
                return resultModel;
            }
            var user = await _userRepository.FindByEmailAsync(email);
            if (user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotFoundError);
                return resultModel;
            }
            return user.Map();
        }

        public async Task<UserModelItem> FindByNameAsync(string userName)
        {
            var resultModel = new UserModelItem();
            if (string.IsNullOrWhiteSpace(userName))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNameIsEmptyError);
                return resultModel;
            }
            var user = await _userRepository.FindByNameAsync(userName);
            if (user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotFoundError);
                return resultModel;
            }
            var role = await CheckRoleAsync(user.Id);
            resultModel.Role = role.Name;
            return user.Map();
        }

        private async Task<Role> CheckRoleAsync(long userId)
        {
            return await _userRepository.CheckRoleAsync(userId);
        }

        public async Task<BaseModel> Register(UserModelItem user)
        {
            var resultModel = new BaseModel();
            if(user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserModelItemIsEmptyError);
                return resultModel;
            }

            var applicationUser = user.Map();

            var result = await _userRepository.CreateAsync(applicationUser);
            if (!result)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotCreatedError);
                return resultModel;
            }
            string token = await _userRepository.GenerateEmailConfirmationTokenAsync(applicationUser);
            if(string.IsNullOrWhiteSpace(token))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.EmailConfirmationTokenNotGeneratedError);
                return resultModel;
            }
            await _emailHelper.Send(user.Email, 
                string.Format("Confirm the registration by clicking on the link: <a href='" +
                Constants.EmailConstants.ConfirmEmail + "'>link</a>", user.Id, token));
            return resultModel;
        }

        public async Task<BaseModel> ConfirmEmail(string userId, string token)
        {
            var resultModel = new BaseModel();
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.TokenOrUserIdIsEmptyError);
                return resultModel;
            }
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotFoundError);
                return resultModel;
            }
            var result = await _userRepository.ConfirmEmailAsync(user, token.CheckGap());
            if(!result)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.EmailNotConfirmedError);
                return resultModel;
            }
            return resultModel;
        }

        public async Task<BaseModel> ForgotPassword(string userEmail)
        {
            var resultModel = new BaseModel();
            if(string.IsNullOrWhiteSpace(userEmail))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserEmailIsEmptyError);
                return resultModel;
            }
            var user = await _userRepository.FindByEmailAsync(userEmail);
            if (user == null)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.UserNotFoundError);
                return resultModel;
            }
            var token = await _userRepository.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrWhiteSpace(token))
            {
                resultModel.Errors.Add(Constants.ErrorConstants.PasswordResetTokenNotGeneratedError);
                return resultModel;
            }
            var password = _createPasswordHelper.CreatePassword(8);
            var result = await _userRepository.ResetPasswordAsync(user, token, password);
            if (!result)
            {
                resultModel.Errors.Add(Constants.ErrorConstants.PasswordNotChangedError);
                return resultModel;
            }
            await _emailHelper.Send(userEmail, string.Format("New password: {0}", password));
            return resultModel;
        }

        //todo remove if this method doesn't use
        public async Task<bool> CheckUserAsync(UserModelItem user, string password, bool lockoutOnFailure) //todo return BaseModel
        {
            return await _userRepository.CheckUserAsync(user.Map(), password, lockoutOnFailure); //todo check result from repo
        }

        //todo remove if this method doesn't use
        public async Task SignInAsync(UserModelItem user, bool isPersistent)
        {
            await _userRepository.SignInAsync(user.Map(), isPersistent);
        }
    }
}
