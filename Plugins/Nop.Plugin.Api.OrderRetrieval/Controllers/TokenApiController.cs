using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Plugin.Api.OrderRetrieval.Models;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Api.OrderRetrieval.Controllers;

[ApiController]
[Route("api/token")]
public class TokenApiController: BaseController
{
    [AllowAnonymous]
    [HttpPost("generate")]
    public IActionResult GenerateToken([FromBody] TokenRequestModel model)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("yoursecretkey12345678901234567890"); // match with Startup.cs

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, model.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { token = tokenString });
    }

}