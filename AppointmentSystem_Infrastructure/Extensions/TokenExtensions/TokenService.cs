using AppointmentSystem_Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentSystem_Infrastructure.Extensions.TokenExtensions
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration; // IConfiguration AppSettings gibi ayarları okumak için gereklidir.
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // Kullanıcı Adı
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // User ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("FirstName", user.Name),
                new Claim("LastName", user.Surname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("TC", user.TC),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())// Token ID

            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(3),
                SigningCredentials = creds,
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
