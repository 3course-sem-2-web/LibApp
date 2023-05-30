namespace webapi.DTOs;

public class UserDTO
{
    public UserDTO(string username, string role)
    {
        Username = username;
        Role = role;
    }

    public string Username { get; }
    public string Role { get; }
}
