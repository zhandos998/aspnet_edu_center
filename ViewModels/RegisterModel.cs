using System.ComponentModel.DataAnnotations;

namespace aspnet_edu_center.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не указан tel num")]
        public string Tel_num { get; set; }
        [Required(ErrorMessage = "Не указан role id")]
        public int Role_id { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
    }
}