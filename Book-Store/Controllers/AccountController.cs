﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookStore.Helper.Interface;
using BookStore.BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookStore.BusinessLogic.Models.Base;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServise _accountService;
        private readonly IJwtHelper _jwtHelper;

        public AccountController(IAccountServise accountService, IJwtHelper jwtHelper)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var resultModel = new BaseModel();
            var user = await _accountService.FindByNameAsync(userName);

            var encodedJwt = await _jwtHelper.GenerateTokenModel(user);

            if (encodedJwt == null)
            {
                resultModel.Errors.Add("some error");
                return Ok(resultModel);
            }

            HttpContext.Response.Cookies.Append("accessToken", encodedJwt.AccessToken);
            HttpContext.Response.Cookies.Append("refreshToken", encodedJwt.RefreshToken);
            return Ok(resultModel);
        }

        [HttpGet("register")]
        public async Task<IActionResult> Register()
        {
            await _accountService.Register();
            return Ok(new BaseModel());
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            return Ok(await _accountService.ConfirmEmail(userId, token));
        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> ForgotPassword()
        {
            return Ok(await _accountService.ForgotPassword("oleksandr.pecherskikh@gmail.com"));
        }
        
        //public async Task<IActionResult> CheckJwtToken(string accessToken, string refreshToken)
        //{
        //    var expiresAccess = new JwtSecurityTokenHandler().ReadToken(accessToken).ValidTo;

        //    if (expiresAccess < DateTime.Now)
        //    {
        //        await RefreshToken(refreshToken);
        //    }
        //    return Ok();
        //}

        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var expires = new JwtSecurityTokenHandler().ReadToken(refreshToken).ValidTo; //check token from jwtHelper

            if (expires >= DateTime.Now)
            {
                var userId = this.HttpContext.User.Claims
               .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _accountService.FindByNameAsync(userId);
                var encodedJwt = await _jwtHelper.GenerateTokenModel(user);
            }
            return Ok(new BaseModel());
        }   
    }
}