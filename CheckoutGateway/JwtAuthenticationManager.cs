using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheckoutGateway
{
    public class JwtAuthenticationManager
    {
        private readonly string key;
        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        {
            { "admin", "admin" },
            { "test", "test" },
        };
        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(string userName, string password)
        {
            if (!users.Any(u => u.Key == userName && u.Value == password))
                return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
