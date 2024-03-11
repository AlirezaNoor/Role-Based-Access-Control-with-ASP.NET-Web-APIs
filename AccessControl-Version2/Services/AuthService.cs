using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccessControl_Version2.Data;
using AccessControl_Version2.Dto;
using AccessControl_Version2.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AccessControl_Version2.Services;

public class AuthService : IAuthService
{
    private readonly MyContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;

    public AuthService(MyContext dbContext, IConfiguration configuration, UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<LoginUser> Login(LoginUser loginUser)
    {
   
        var userresult = await _userManager.FindByNameAsync(loginUser.UserName);
        if (userresult!=null)
        {
 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);
            var GetRole =await _userManager.GetRolesAsync(userresult);
            // Prepare list of user claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.UserName),
                new Claim(ClaimTypes.GivenName, loginUser.UserName+"@Mail.com"),
                // new Claim(ClaimTypes.Role,loginUser.Role)
            };
            foreach (var  role in GetRole)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            // Create token descriptor

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.UtcNow,
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            // Create token and set it to user

            var token = tokenHandler.CreateToken(tokenDescriptor);
            loginUser.Token = tokenHandler.WriteToken(token);
            loginUser.IsActive = true;

            return loginUser;
        }

        return new LoginUser();

    }

    public async Task<RegisterUser> Register(RegisterUser registerUser)
    {
        User user = new User()
        {
            UserName = registerUser.UserName,
            Email = registerUser.UserName + "@Mail.com",
            NormalizedUserName = registerUser.UserName.ToUpper(),
            NormalizedEmail = registerUser.UserName + "@Mail.com".ToUpper(),
            
        };


        var result =await _userManager.CreateAsync(user, registerUser.Password);
        if (result.Succeeded)
        {
            var roles=await _userManager.AddToRoleAsync(user, registerUser.Roles);
        }
        return registerUser;
    }
}