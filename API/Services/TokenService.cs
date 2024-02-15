using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key; // symmetric keys are used to both decrypt and encrypt tokens since we do both on the server.
    // we store the key in our configuration - we need to inject an IConfiguration
    public TokenService(IConfiguration config)
    {
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); // we'll make a TokenKey entry in our appsettings json files
    }
    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName) // token will claim what the username is
        };
        // signing the token
        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // describe token we're going to return
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7), // normally you would have a shorter duration in production
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        // create the token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
