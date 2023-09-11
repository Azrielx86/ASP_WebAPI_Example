using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Models;

public class LoginViewModel
{
    [Required] 
    [EmailAddress]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Password { get; set; }

    [Display(Name = "Recuerdame")]
    public bool RememberMe { get; set; }
}