using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InnowiseFridge_project.Data;
using InnowiseFridge_project.Interfaces.ServiceInterfaces;
using Microsoft.IdentityModel.Tokens;

namespace InnowiseFridge_project.Services;

public class TokenService : ITokenService
{
    private IConfiguration Configuration { get; }

    public TokenService() { }
    public TokenService(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public string GenerateJwtToken(string userId, string userName, Role role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        switch (role)
        {
            case Role.Admin:
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                break;
            case Role.FridgeOwner:
                claims.Add(new Claim(ClaimTypes.Role, "FridgeOwner"));
                break;
            case Role.Authorized:
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                break;
        }

        var token = new JwtSecurityToken(
            Configuration["Jwt:Issuer"],
            Configuration["Jwt:Issuer"],
            claims,
            expires: DateTime.Now.AddDays(365),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}