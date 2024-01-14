using CustomerService.API.Logic;
using CustomerService.API.Models.Auth;
using CustomerService.API.Models.DTO;
using CustomerService.API.Models.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CustomerService.API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerLogic _customerlogic;

        public CustomerController(ICustomerLogic customerLogic)
        {
            _customerlogic = customerLogic;
        }

        [HttpPost("create"), AllowAnonymous]
        public async Task<CustomerAuthDto> Create(CustomerAuthDto customerAuthDTO)
        {
            return await _customerlogic.CreateCustomerLogic(customerAuthDTO);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(CustomerAuthDto customerAuthDTO)
        {
            var refreshToken = CreateRefreshToken();
            SetRefreshToken(refreshToken);
            AuthResponse dto = await _customerlogic.LoginCustomerLogic(customerAuthDTO, refreshToken);

            return Ok(dto);
        }

        [HttpPut("update"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> Update(String customer)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPut("updateadmin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateAdmin(String customer)
        {
            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken(int id)
        {
            var refreshToken = Request.Cookies["refreshtoken"];

            CustomerEntity customer = await _customerlogic.GetCustomer(id);

            if (!refreshToken.Equals(customer.RefreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if(customer.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired");
            }

            string token = _customerlogic.CreateToken(customer).Result.JwtToken;
            var newRefreshToken = CreateRefreshToken();
            SetRefreshToken(newRefreshToken);

            customer.RefreshToken = newRefreshToken.Token;
            customer.TokenCreated = newRefreshToken.Created;
            customer.TokenExpires = newRefreshToken.Expires;

            await _customerlogic.SetRefreshToken(customer);

            return Ok(token);
        }

        [HttpGet("personaldata"), Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerEntity>> GetPersonalData(int id)
        {
            return Ok(await _customerlogic.GetCustomer(id));
        }

        [HttpGet("delete"), Authorize(Roles = "Customer")]
        public async Task<ActionResult> DeleteAccount(int id)
        {
            await _customerlogic.DeleteAccount(id);
            return Ok();
        }

        private static RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}