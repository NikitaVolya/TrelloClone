using System.ComponentModel.DataAnnotations;

namespace MVC.Models.Profile
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Введіть поточний пароль")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Введіть новий пароль")]
        [MinLength(6, ErrorMessage = "Пароль повинен містити щонайменше 6 символів")]
        [DataType(DataType.Password)]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).+$",
            ErrorMessage = "Пароль повинен містити хоча б одну велику літеру та один спеціальний символ"
        )]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Підтвердіть новий пароль")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Паролі не співпадають")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}
