using System;
using System.ComponentModel.DataAnnotations;

namespace Notes.Web.Models
{
    public class ReminderModel
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        public virtual string Description { get; set; }

        [Display(Name = "Сделать до")]
        [DataType(DataType.DateTime, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime? TimeToAchieve { get; set; }

    }
}