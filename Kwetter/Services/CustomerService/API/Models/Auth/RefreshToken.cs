﻿namespace CustomerService.API.Models.Auth
{
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires {  get; set; } = DateTime.Now.AddDays(7);
    }
}
