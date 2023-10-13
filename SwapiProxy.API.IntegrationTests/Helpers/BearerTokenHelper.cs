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
            var issuer = "https://test-issuer.com/";
            var audience = "https://test-audience.com/";
            var key = Encoding.ASCII.GetBytes("nlkjn234jn2lk4jn2lkjn2lkjnk4hj2fchgfc4l2jkb42g4v2gf4v2lkj3n42kljhb4j2hg4vk2jh4b23kj4n23jg4vhg4v23kjh4b2lk4n23lkj4n23lk4hb2kgv4");

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
