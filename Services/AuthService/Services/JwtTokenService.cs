using AuthService.Dtos;
using AuthService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public RefreshToken GenerateRefreshToken(Guid tenantId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            TenantId = tenantId
        };
    }

    public string GenerateToken(User user, string role, Guid tenantId)
    {
        // Implementation for generating JWT token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("firstName", user.FirstName),
                new("lastName", user.LastName),
                new("tenantId", tenantId.ToString()),
                new(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Task<AuthResponse> RefreshTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AuthResponse> RevokeTokenAsync()
    {
        throw new NotImplementedException();
    }

    public bool ValidateToken(string token, out Guid userId, out string email, out IEnumerable<string> roles)
    {
        // Implementation for validating JWT token

        throw new NotImplementedException();
    }
}
