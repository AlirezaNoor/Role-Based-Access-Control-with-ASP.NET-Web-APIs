namespace AccessControl_Version2.Dto;

public class LoginUser
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public string Token { get; set; } = "";
    public bool IsActive { get; set; } = true;
    
}