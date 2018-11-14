using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Notes.Web.Models
{
    public class CreateNoteModel
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите название заметки")]
        public string Title { get; set; }

        [Display(Name = "Список тегов")]
        public string[] Tags { get; set; }

        [Display(Name = "Опуликовать")]
        public bool Published { get; set; }

        [Display(Name = "Текст заметки")]
        [Required(ErrorMessage = "Введите текст заметки")]
        public string Text { get; set; }

        [Display(Name = "Прикрепить файл")]
        public HttpPostedFileBase BinaryFile { get; set; }
    }
}