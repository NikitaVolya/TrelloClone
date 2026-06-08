using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Введіть пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}
