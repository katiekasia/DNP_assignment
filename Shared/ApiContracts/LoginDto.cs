namespace ApiContracts;

public class LoginDto
{
    public LoginDto(String Username, String Password)
    {
        this.Username = Username;
        this.Password = Password;
    }
    public String Username { get; set; }
    public String Password { get; set; }

}