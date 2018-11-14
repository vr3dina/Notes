using Notes.DB;
using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System;
using System.IO;
using System.Web.Mvc;

namespace Notes.Web.Controllers
{
    public class NoteController : Controller
    {
        private INoteRepository noteRepository;
        private IUserRepositoty userRepositoty;

        public NoteController()
        {
            noteRepository = new NHNoteRepository();
            userRepositoty = new NHUserRepository();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateNoteModel model)
        {
            User user = userRepositoty.Load(1);

            string fileType = "";
            byte[] fileData = null;
            var file = model.BinaryFile;

            if (file != null)
            {
                fileType = file.ContentType;

                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }
            }

            noteRepository.Save(new Note()
            {
                Title = model.Title,
                Published = model.Published,
                Text = model.Text,
                Tags = model.Tags,
                CreationDate = DateTime.Now,
                User = user,
                BinaryFile = fileData,
                FileType = fileType
            });

            return RedirectToAction("ShowPublishedNotes");
        }

        public PartialViewResult ShowPublishedNotes()
        {
            var allNotes = noteRepository.LoadAllPublished();
            return PartialView("ShowNotes", allNotes);
        }

        public ActionResult Download(int id)
        {
            var note = noteRepository.Load(id);
            return File(note.BinaryFile, note.FileType);
        }
    }
}