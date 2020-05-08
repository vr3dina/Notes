using System;
using System.ComponentModel.DataAnnotations;

namespace Notes.DB
{
    public class Reminder : IEntity
    {
        public virtual long Id { get; set; }

        [Display(Name = "Название")]
        public virtual string Title { get; set; }

        [Display(Name = "Выполнено")]
        public virtual bool IsDone { get; set; }

        [Display(Name = "Описание")]
        public virtual string Description { get; set; }

        [Display(Name = "Сделать до")]
        [DataType(DataType.DateTime, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime? TimeToAchieve { get; set; }

        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }
    }
}