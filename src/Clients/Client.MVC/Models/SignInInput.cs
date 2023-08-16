using System.ComponentModel.DataAnnotations;

namespace Client.MVC.Models;

public class SignInInput
{
    [Required]
    [Display(Name = "Your Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Your Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool IsRemember { get; set; }
}
