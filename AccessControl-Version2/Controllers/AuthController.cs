using System.IdentityModel.Tokens.Jwt;
using AccessControl_Version2.Dto;
using AccessControl_Version2.Entities;
using AccessControl_Version2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl_Version2.Controllers;

 [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: auth/login
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUser user)
        {
            // Error checks

            if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try login

            var loggedInUser = await _authService.Login(user);

            // Return responses

            if (loggedInUser != null)
            {
                return Ok(loggedInUser);
            }

            return BadRequest(new { message = "User login unsuccessful" });
        }

        // POST: auth/register
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            // Error checks

 
            if (String.IsNullOrEmpty(user.UserName))
            {
                return BadRequest(new { message = "User name needs to entered" });
            }
            else if (String.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new { message = "Password needs to entered" });
            }

            // Try registration

            var registeredUser = await _authService.Register(user);

            // Return responses

            if (registeredUser != null)
            {
                return Ok(registeredUser);
            }

            return BadRequest(new { message = "User registration unsuccessful" });
        }

        // GET: auth/test
       
        [HttpGet]
        public IActionResult Test()
        {
            // Get token from header
            ResponesDto ResponesDto = new ResponesDto();
            var Edited = User.Identity.Name;
         
                string token = Request.Headers["Authorization"];

                if (token.StartsWith("Bearer"))
                {
                    token = token.Substring("Bearer ".Length).Trim();
                }
                var handler = new JwtSecurityTokenHandler();

                // Returns all claims present in the token

                JwtSecurityToken jwt = handler.ReadJwtToken(token);

                var claims = "List of Claims: \n\n";

                foreach (var claim in jwt.Claims)
                {
                    claims += $"{claim.Type}: {claim.Value}\n";
                }

                ResponesDto.flag = true;
                ResponesDto.Message = "this claims";
                ResponesDto.Data = claims;
                return Ok(ResponesDto);
          

            return Unauthorized();
        }
    }