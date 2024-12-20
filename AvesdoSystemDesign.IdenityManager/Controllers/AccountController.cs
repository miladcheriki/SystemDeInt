using AvesdoSystemDesign.Shared;
using AvesdoSystemDesign.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AvesdoSystemDesign.IdenityManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IOptions<JwtSettings> options) : ControllerBase
{
    [HttpPost("token")]
    public IActionResult GetToken(string email)
    {
        // Simple User Validation
        if (!Helpers.IsValidEmail(email))
            return Unauthorized("Invalid credentials");

        var token = JwtTokenHelper.GenerateToken(email, options.Value);
        return Ok(new { Token = token });
    }
}