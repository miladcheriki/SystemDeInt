using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AvesdoSystemDesign.Shared.Models;
using Microsoft.IdentityModel.Tokens;

namespace AvesdoSystemDesign.IdenityManager;

public static class JwtTokenHelper
{
    public static string GenerateToken(string email, JwtSettings jwtSettings)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), 
            signingCredentials: creds
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}