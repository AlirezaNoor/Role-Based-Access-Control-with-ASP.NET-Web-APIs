using AccessControl.Entity.Users;

namespace AccessControl.Services;

public interface IAuthService
{
    public Task<User> Login(User loginUser);
    public Task<User> Register(User registerUser);
}