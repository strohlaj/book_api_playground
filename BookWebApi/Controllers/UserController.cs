using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookWebApi.Contracts;
using BookWebApi.Services;

namespace BookWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : Controller
{
    private readonly IRegisterService _registerService;

    public UserController(IRegisterService registerService)
    {
        _registerService = registerService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Register([FromBody] RegisterInfo registerInfo)
    {
        // Todo: return detailed error result from failure to register.
        return _registerService.RegisterUserAsAuthor(registerInfo) ? Ok() : ValidationProblem();
    }
}    
