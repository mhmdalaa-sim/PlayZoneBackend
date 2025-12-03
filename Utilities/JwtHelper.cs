using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PlayZone.Configuration;

namespace PlayZone.Utilities;

public static class JwtHelper
{
    public static string GenerateToken(Guid userId, string username, JwtSettings settings)
    {
   var tokenHandler = new JwtSecurityTokenHandler();
   var key = Encoding.ASCII.GetBytes(settings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
Subject = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
    new Claim(ClaimTypes.Name, username),
  new Claim(ClaimTypes.Role, "Admin")
    }),
  Expires = DateTime.UtcNow.AddHours(settings.ExpirationHours),
    Issuer = settings.Issuer,
 Audience = settings.Audience,
SigningCredentials = new SigningCredentials(
   new SymmetricSecurityKey(key),
       SecurityAlgorithms.HmacSha256Signature)
   };

var token = tokenHandler.CreateToken(tokenDescriptor);
  return tokenHandler.WriteToken(token);
 }

  public static ClaimsPrincipal? ValidateToken(string token, JwtSettings settings)
    {
  try
      {
       var tokenHandler = new JwtSecurityTokenHandler();
       var key = Encoding.ASCII.GetBytes(settings.Secret);

        var validationParameters = new TokenValidationParameters
       {
      ValidateIssuerSigningKey = true,
  IssuerSigningKey = new SymmetricSecurityKey(key),
   ValidateIssuer = true,
  ValidIssuer = settings.Issuer,
   ValidateAudience = true,
      ValidAudience = settings.Audience,
      ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero
     };

     var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
    return principal;
   }
     catch
        {
     return null;
  }
 }
}
