using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using StoreManagementSystem.Models.Domains;
using StoreManagementSystem.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StoreManagementSystem.Services
{
    public class HelperService : IHelperService
    {
        private readonly IConfiguration _configuration;
        public HelperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Authenticate(User user, bool IsSuperAdmin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var claims = new Dictionary<string, object>(){
                { ClaimTypes.Name, user.Name },
                { ClaimTypes.Email, user.EmailAddress }

            };

            if (IsSuperAdmin)
                claims.Add(ClaimTypes.Role, "SuperAdmin");
            else if (user.IsAdmin)
                claims.Add(ClaimTypes.Role, "Admin");
            else
                claims.Add(ClaimTypes.Role, "User");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("sub", user.Id.ToString())
                }),

                Claims = claims,
                // Expires = DateTime.Now.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string PasswordSalt(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_configuration["PasswordSalt"].ToString());

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                ));
            return hashed;
        }

    }
}
