using CustomerService.API.Eventing.EventPublisher.CustomerCreated;
using CustomerService.API.Models.DTO;
using CustomerService.API.Models.Entity;
using CustomerService.API.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using CustomerService.API.Models.Auth;

namespace CustomerService.API.Logic
{
    public class CustomerLogic : ICustomerLogic
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _repository;
        private readonly IConfiguration _configuration;

        public CustomerLogic(IMediator mediator, ICustomerRepository repository, IConfiguration configuration)
        {
            _mediator = mediator;
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<CustomerAuthDto> CreateCustomerLogic(CustomerAuthDto customerAuthDTO)
        {
            CustomerEntity customerEntity = CreatePasswordHash(customerAuthDTO, out _, out _);

            if(await _repository.GetByName(customerAuthDTO.CustomerName) != default)
            {
                customerAuthDTO.CustomerName = "name already taken";
                return customerAuthDTO;
            }
            customerEntity = await _repository.Create(customerEntity);

            var customer = new CustomerCreatedEvent
            {
                CustomerId = customerEntity.Id,
                CustomerName = customerEntity.CustomerName,
                DisplayName = customerEntity.DisplayName,
            };

            await _mediator.Send(customer);

            customerAuthDTO.Password = "***";

            return customerAuthDTO;
        }

        public async Task<AuthResponse> LoginCustomerLogic(CustomerAuthDto customerAuthDTO, RefreshToken refreshToken)
        {
            CustomerEntity customerEntity = await _repository.GetByName(customerAuthDTO.CustomerName);

            customerEntity.RefreshToken = refreshToken.Token;
            customerEntity.TokenCreated = refreshToken.Created;
            customerEntity.TokenExpires = refreshToken.Expires;

            await _repository.Update(customerEntity);

            if(customerEntity.PasswordHash == null) 
            {
                return new AuthResponse();
            }

            if(!VerifyPasswordHash(customerAuthDTO.Password, customerEntity.PasswordHash, customerEntity.PasswordSalt))
            {
                return new AuthResponse();
            }

            return await CreateToken(customerEntity);
        }

        private CustomerEntity CreatePasswordHash(CustomerAuthDto customerAuthDTO, out byte[] passwordHash, out byte[] passwordSalt) 
        { 
            using(var hmac = new HMACSHA256()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(customerAuthDTO.Password));
            }

            return new(customerAuthDTO.CustomerName, customerAuthDTO.CustomerName, passwordHash, passwordSalt);
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<AuthResponse> CreateToken(CustomerEntity customer)
        {
            await Task.CompletedTask;
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.CustomerName),
                new Claim(ClaimTypes.Role, customer.Role),
                new Claim("Id", customer.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );

            int[] roles = new int[1];

            if(customer.Role == "Admin")
            {
                roles[0] = 2;
                return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), roles, customer.Id);
            }

            roles[0] = 1;
            return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), roles, customer.Id);
        }

        public async Task<CustomerEntity> GetCustomer(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task SetRefreshToken(CustomerEntity customer)
        {
            await _repository.Update(customer);
        }
    }
}
