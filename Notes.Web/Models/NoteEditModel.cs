using System;
using System.ComponentModel.DataAnnotations;

namespace Notes.Web.Models
{
    public class NoteEditModel
    {
        public long Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название заметки")]
        public string Title { get; set; }

        [Display(Name = "Список тегов")]
        public string[] Tags { get; set; }

        [Display(Name = "Опуликовать")]
        public bool Published { get; set; }
        
        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime CreationDate { get; set; }

        [Display(Name = "Текст заметки")]
        [Required(ErrorMessage = "Введите текст заметки")]
        public string Text { get; set; }

        [Display(Name = "Прикрепить файл")]
        public DB.File BinaryFile { get; set; }
    }
}