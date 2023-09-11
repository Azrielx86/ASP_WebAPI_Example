using LoginAPI.Models;
using LoginAPI.Models.Auth;
using LoginAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtService _jwtService;

    public UserController(UserManager<IdentityUser> userManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("BearerToken")]
    public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
    {
        if (!ModelState.IsValid) return BadRequest("Bad Credentials.");
        var user = await _userManager.FindByNameAsync(request.UserName!);
        if (user is null) return BadRequest("Bad Credentials");
        var pwdValid = await _userManager.CheckPasswordAsync(user, request.Password!);
        if (!pwdValid) return BadRequest("Bad credentials.");
        var token = _jwtService.CreateToken(user);
        return Ok(token);
    }

    // [Authorize]
    [HttpGet("msg")]
    public string Msg() => "It works!";

    // [HttpPost]
    // [AllowAnonymous]
    // [Route("login")]
    // public async Task<IActionResult> Login(LoginViewModel model)
    // {
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);
    //
    //     var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe,
    //         lockoutOnFailure: false);
    //
    //     if (result.Succeeded)
    //         return RedirectToAction("Msg");
    //
    //     ModelState.AddModelError(string.Empty, "Incorrect User or Password.");
    //     return BadRequest(model);
    // }

    [HttpPost]
    [AllowAnonymous]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
    
        var user = new IdentityUser() { Email = model.Email, UserName = model.Email };
        var result = await _userManager.CreateAsync(user, password: model.Password!);
    
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Created(string.Empty, user);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("autherror")]
    public string AuthError() => "You are not authorized! Baka!";
}