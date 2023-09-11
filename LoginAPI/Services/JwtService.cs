using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using LoginAPI.Models.Auth;

namespace LoginAPI.Services;

public class JwtService
{
    private const int ExpirationMinutes = 1;
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AuthenticationResponse CreateToken(IdentityUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSignInCredentials(),
            expiration);

        var tokenHandler = new JwtSecurityTokenHandler();

        return new AuthenticationResponse
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = expiration
        };
    }

    private JwtSecurityToken CreateJwtToken(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
            claims, expires: expiration, signingCredentials: credentials);

    private IEnumerable<Claim> CreateClaims(IdentityUser user) => new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!)
    };

    private SigningCredentials CreateSignInCredentials() =>
        new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            SecurityAlgorithms.HmacSha256);
}