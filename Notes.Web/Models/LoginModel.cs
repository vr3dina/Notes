using System.ComponentModel.DataAnnotations;

namespace Notes.Web.Models
{
    public class LoginModel
    {
        //[Required]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [Display(Name ="Логин")]
        public string Login { get; set; }

        //[Required]
        [Required(ErrorMessage = "Это поле обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}