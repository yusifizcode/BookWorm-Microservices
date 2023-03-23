using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models;

public class SignInInput
{
    [Display(Name = "Your Email")]
    public string Email { get; set; }
    [Display(Name = "Your Password")]
    public string Password { get; set; }
    [Display(Name = "Remember me")]
    public bool Remember { get; set; }
}
