using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookWebApi.Models;
using BookWebApi.Contracts;
using BookWebApi.Services;

namespace BookWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : Controller
{
    private readonly IConfiguration _config;
    private readonly IAuthenticationService _authService;

    public LoginController(IConfiguration config, IAuthenticationService authService)
    {
        _config = config;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] UserInfo login)
    {
        IActionResult response = Unauthorized();
        var (user, userId) = _authService.AuthenticateUser(login);

        if (user is not null)
        {
            var tokenString = GenerateJSONWebToken(user, userId);
            response = Ok(new { token = tokenString });
        }
        return response;
    }

    private string GenerateJSONWebToken(User user, Guid userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
          issuer: _config["Jwt:Issuer"],
          audience: _config["Jwt:Issuer"],
          // TODO: Encrypt sensitive user information since we're using the jwt as a poor-mans session state manager
          claims: new[] { new Claim("Username", user.Username), new Claim("UserId", userId.ToString())},
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}    
