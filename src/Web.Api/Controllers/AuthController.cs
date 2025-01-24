using Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase, IAuthAppService
{
    private readonly IAuthAppService _authAppService;

    public AuthController(IAuthAppService authAppService)
    {
        _authAppService = authAppService;
    }

    [Route("login")]
    [HttpPost]
    public async Task<LoginResultDto> LoginAsync([FromBody] LoginDto loginDto)
    {
        return await _authAppService.LoginAsync(loginDto);
    }

    [Route("register")]
    [HttpPost]
    public async Task RegisterAsync([FromBody] RegisterDto registerDto)
    {
        await _authAppService.RegisterAsync(registerDto);
    }
}
