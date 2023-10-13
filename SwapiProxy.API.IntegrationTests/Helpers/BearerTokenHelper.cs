using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SwapiProxy.API.IntegrationTests.Helpers
{
    internal static class BearerTokenHelper
    {
        internal static string GetBearerToken()
        {
            var issuer = "https://issuer.com/";
            var audience = "https://audience.com/";
            var key = Encoding.ASCII.GetBytes("jnk2j3n4l2kj4n2lkj34nl2kj4n2lkj4nl2kjnl2j4nlk2jb4hg2cvj2fc4h2fgc4vlk2j3n4ksdfsdfsdfsdfsjfjaddgskyergbfjrgadgjnmxghjidgfdh");

            var subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, "test-user"),
                new Claim(JwtRegisteredClaimNames.Email, "email@user.com"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
