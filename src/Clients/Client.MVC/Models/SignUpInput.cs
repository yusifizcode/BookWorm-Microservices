using System.ComponentModel.DataAnnotations;

namespace Client.MVC.Models;

public class SignUpInput
{
    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; }
    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Display(Name = "City")]
    public string City { get; set; }
}
