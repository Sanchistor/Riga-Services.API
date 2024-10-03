using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using riga.services.DbContext;
using riga.services.Models;
using riga.services.riga.services.authentication.Commands;
using riga.services.riga.services.authentication.DTO;
using riga.services.riga.services.authentication.Responses;
using riga.services.riga.services.authentication.Services;

namespace riga.services.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtAuthenticationManager jwtAuthenticationManager;
    private readonly ApiDbContext _context;
    private readonly IMediator _mediator;
    private readonly IPasswordService _passwordService;
    public AuthController(JwtAuthenticationManager jwtAuthenticationManager, ApiDbContext context, IMediator mediator, IPasswordService passwordService)
    {
        this.jwtAuthenticationManager = jwtAuthenticationManager;
        _context = context;
        _mediator = mediator;
        _passwordService = passwordService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> CreateUserAndProfile(CreateProfileDto createProfileDto)
    {
        var command = new CreateUserCommand { createProfileDto = createProfileDto };
        await _mediator.Send(command);
        return Ok("User and Profile created successfully!");
    }

    [AllowAnonymous]
    [HttpPost("Authorize")]
    public ActionResult<TokenResponse> AuthUser(UserLoginDto userLoginDto)
    {
        User user = _context.Users.FirstOrDefault(u => u.Email == userLoginDto.email);

        if (user == null)
        {
            return BadRequest(new { message = "User not found" });
        }

        if (!_passwordService.VerifyPassword(userLoginDto.password, user.Password))
        {
            return Unauthorized(new { message = "Invalid password" });
        }

        var key = "Yh2k7QSu418CZg5p6X3Pna9L0Miy4D3Bvt0JVr87Uc0j69Kqw5R2Nmf4FWs03Hdx";
        JwtAuthenticationManager jwtAuthenticationManager = new JwtAuthenticationManager(key, _context);
        string token = jwtAuthenticationManager.Authenticate(user.Email, userLoginDto.password);

        return Ok(new { Token = token, Message = "Authentication successful!" });
    }
}