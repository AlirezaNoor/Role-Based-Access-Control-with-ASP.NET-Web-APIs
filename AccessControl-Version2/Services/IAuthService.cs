 

using AccessControl_Version2.Dto;
using AccessControl_Version2.Entities;

namespace AccessControl_Version2.Services;

public interface IAuthService
{
    public Task<LoginUser> Login(LoginUser loginUser);
    public Task<RegisterUser> Register(RegisterUser registerUser);
}