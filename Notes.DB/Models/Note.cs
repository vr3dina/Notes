using System;
using System.ComponentModel.DataAnnotations;

namespace Notes.DB
{
    public class Note : IEntity
    {
        public virtual long Id { get; set; }

        [Display(Name = "Название")]
        public virtual string Title { get; set; }

        [Display(Name = "Опубликовано")]
        public virtual bool Published { get; set; }

        [Display(Name = "Текст")]
        public virtual string Text { get; set; }

        [Display(Name = "Теги")]
        public virtual string Tags { get; set; }

        [Display(Name = "Дата создания")]
        public virtual DateTime CreationDate { get; set; }

        [Display(Name = "Пользователь")]
        public virtual User User { get; set; }

        [Display(Name = "Файл")]
        public virtual byte[] BinaryFile { get; set; }

        public virtual string FileType { get; set; }
    }
}