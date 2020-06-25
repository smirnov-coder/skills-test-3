using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace WebUI.Helpers
{
    /// <inheritdoc cref="IJwtHelper"/>
    public class JwtHelper : IJwtHelper
    {
        private JwtOptions _options;
        private JwtSecurityTokenHandler _tokenHandler;

        public JwtHelper(JwtOptions options = null, JwtSecurityTokenHandler tokenHandler = null)
        {
            _options = options ?? JwtOptions.Default;
            _tokenHandler = tokenHandler ?? new JwtSecurityTokenHandler();
        }

        public bool ValidateToken(string encodedJwt)
        {
            _tokenHandler.ValidateToken(encodedJwt, GetValidationParameters(), out SecurityToken token);
            return token != null;
        }

        public TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _options.GetSymmetricSecurityKey(),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }

        public string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    // Будем хранить в JWT только UserName, остальное нас не интересует.
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.Add(_options.Lifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
