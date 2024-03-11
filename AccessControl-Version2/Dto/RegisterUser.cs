namespace AccessControl_Version2.Dto;

public class RegisterUser
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public  string  Roles { get; set; }
}