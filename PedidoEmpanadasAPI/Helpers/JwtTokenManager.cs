using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Shared.Models;

namespace PedidoEmpanadasAPI.Helpers
{
    public class JwtTokenManager
    {
        private readonly ILogger<JwtTokenManager> _logger;

        public JwtTokenManager(ILogger<JwtTokenManager> logger)
        {
            _logger = logger;
        }

        public string GenerateJwtToken(User user, IConfiguration configuration)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };

                int TokenExpiration = Convert.ToInt32(configuration["JwtSettings:TokenExpirationMinutes"]);

                var token = new JwtSecurityToken(
                    issuer: configuration["JwtSettings:Issuer"],
                    audience: configuration["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(TokenExpiration),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return "";
            }

        }


        public bool ValidateToken(string token, IConfiguration configuration)
        {
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
                        var tokenHandler = new JwtSecurityTokenHandler();

                        tokenHandler.ValidateToken(token, new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidIssuer = configuration["JwtSettings:Issuer"],
                            ValidAudience = configuration["JwtSettings:Audience"],
                            IssuerSigningKey = securityKey,
                            ValidateLifetime = true
                        }, out SecurityToken validatedToken);
                        if (validatedToken is JwtSecurityToken jwtToken)
                        {
                            return true;
                        }
                        return false;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {                
                return false;
            }
        }
    }
}
