using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models.Auth;

public class AuthenticationRequest
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}