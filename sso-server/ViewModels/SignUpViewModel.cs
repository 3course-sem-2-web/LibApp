using System.ComponentModel.DataAnnotations;

namespace sso_server.ViewModels;

public class SignUpViewModel
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string RetryPassword { get; set; } = null!;
}