namespace AccessControl.Dto;

public class RegisterUser
{
    public string Name { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public List<string>? Roles { get; set; }
}