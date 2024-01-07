namespace CustomerService.API.Models.DTO
{
    public class AuthResponse
    {
        public string JwtToken { get; set; }

        public int[] Roles { get; set; }

        public AuthResponse() { }

        public AuthResponse(string jwtToken, int[] roles)
        {
            JwtToken = jwtToken;
            Roles = roles;
        }
    }
}
