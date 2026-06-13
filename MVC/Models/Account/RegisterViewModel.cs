using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введіть ім'я користувача")]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Введіть Email")]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$",
            ErrorMessage = "Пароль повинен містити хоча б одну велику літеру та один спеціальний символ"
        )]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
