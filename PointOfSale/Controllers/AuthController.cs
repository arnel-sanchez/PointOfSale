using PointOfSale.Definitions;
using PointOfSale.Models.AuthModels;
using PointOfSale.Models.DataBaseModels;
using PointOfSale.Services;
using PointOfSale.Services.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PointOfSale.DataAccess;
using PointOfSale.Models;

namespace PointOfSale.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserDataAccess _userDataAccess;
        private readonly IJwtAuthManager _jwtAuthManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(ILogger<AuthController> logger, IJwtAuthManager jwtAuthManager, UserManager<User> userManager, SignInManager<User> signInManager, IUserDataAccess userDataAccess)
        {
            _logger = logger;
            _jwtAuthManager = jwtAuthManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _userDataAccess = userDataAccess;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                var role = _userManager.GetRolesAsync(user).Result.ToList()[0];
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name,request.UserName),
                    new Claim(ClaimTypes.Role, role)
                };
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    return Unauthorized();
                }
                var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
                _logger.LogInformation($"User [{request.UserName}] logged in the system.");
                return Ok(new LoginResult
                {
                    UserName = request.UserName,
                    Role = role,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [Authorize( Roles = Roles.Admin)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var user = new User { UserName = request.UserName,
                    Email = request.Email,
                    Role = request.Role
                };
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, user.Role);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    _logger.LogInformation($"The user [{user.UserName}] has been registered");
                    return Ok();
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                }
                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            try
            {
                if(userId == null)
                {
                    _logger.LogError("UserId can´t be null");
                    return BadRequest("UserId can´t be null");
                }
                if(code == null)
                {
                    _logger.LogError("Code can´t be null");
                    return BadRequest("Code can´t be null");
                }
                var user = await _userManager.FindByIdAsync(userId);
                if(user==null)
                {
                    _logger.LogError($"Not exist an User with the Id [{userId}]");
                    return BadRequest($"Not exist an User with the Id [{userId}]");
                }
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ConfirmEmailAsync(user, code);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    _logger.LogInformation($"User [{user.Name}] has confirmed his email");
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    };
                    var jwtResult = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.Now);
                    return Ok(new LoginResult
                    {
                        UserName = user.UserName,
                        Role = user.Role,
                        AccessToken = jwtResult.AccessToken,
                        RefreshToken = jwtResult.RefreshToken.TokenString
                    });
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        _logger.LogError(item.Description);
                    }
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("confirm-email-change")]
        public async Task<ActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            try
            {
                if (userId == null)
                {
                    _logger.LogError("UserId can´t be null");
                    return BadRequest("UserId can´t be null");
                }
                if (code == null)
                {
                    _logger.LogError("Code can´t be null");
                    return BadRequest("Code can´t be null");
                }
                if (email == null)
                {
                    _logger.LogError("Email can´t be null");
                    return BadRequest("Email can´t be null");
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogError($"Not exist an User with the Id [{userId}]");
                    return NotFound($"Not exist an User with the Id [{userId}]");
                }

                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var result = await _userManager.ChangeEmailAsync(user, email, code);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        _logger.LogError(item.Description);
                    }
                    return BadRequest(result.Errors);
                }
                _logger.LogInformation($"User [{user.Name}] has confirmed his email");
                var claims = new[]
                    {
                        new Claim(ClaimTypes.Name,user.UserName),
                        new Claim(ClaimTypes.Role, user.Role)
                    };
                var jwtResult = _jwtAuthManager.GenerateTokens(user.UserName, claims, DateTime.Now);
                return Ok(new LoginResult
                {
                    UserName = user.UserName,
                    Role = user.Role,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    if (user == null)
                    {
                        _logger.LogError($"No user has registered with the email [{request.Email}]");
                        return BadRequest($"No user has registered with the email [{request.Email}]");
                    }
                    else
                    {
                        _logger.LogError($"The user [{user.UserName}] has not confirmed his email");
                        return BadRequest($"The user [{user.UserName}] has not confirmed his email");
                    }
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                /*var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                
                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Reestablezca su contraseña haciendo click aquí: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Reestablecer Contraseña</a>.");
                */
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var list = new List<string>();
                    foreach (var items in ModelState.Values)
                    {
                        foreach (var errors in items.Errors)
                        {
                            _logger.LogError(errors.ErrorMessage);
                            list.Add(errors.ErrorMessage);
                        }
                    }
                    return BadRequest(list);
                }
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogError($"No user has registered with the email [{request.Email}]");
                    return BadRequest($"No user has registered with the email [{request.Email}]");
                }

                var result = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"The user [{user.UserName}] has changed his password");
                    return Ok();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        _logger.LogError(item.Description);
                    }
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpGet("get-user")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userName = User.Identity?.Name;
                var user = await _userManager.FindByNameAsync(userName);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var userName = User.Identity.Name;
            _jwtAuthManager.RemoveRefreshTokenByUserName(userName);
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"User [{userName}] logged out the system.");
            return Ok();
        }

        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var userName = User.Identity.Name;
                _logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return Unauthorized();
                }

                var accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
                var jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
                _logger.LogInformation($"User [{userName}] has refreshed JWT token.");
                return Ok(new LoginResult
                {
                    UserName = userName,
                    Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
            }
        }

        [HttpGet("get-sellers")]
        public IActionResult GetSellers()
        {
            try
            {
                var users = _userDataAccess.GetSellers();
                return Ok(users);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}