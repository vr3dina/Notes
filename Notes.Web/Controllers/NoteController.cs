using Notes.DB;
using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private INoteRepository noteRepository;
        private IUserRepositoty userRepositoty;

        public NoteController()
        {
            noteRepository = new NHNoteRepository();
            userRepositoty = new NHUserRepository();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NoteModel model)
        {
            User user = userRepositoty.LoadByLogin(User.Identity.Name);

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

            return RedirectToAction("MyNotes");
        }

        public ActionResult PublishedNotes()
        {
            var publicNotes = noteRepository.LoadAllPublished();

            return View(publicNotes);
        }

        public ActionResult MyNotes()
        {
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            var myNotes = noteRepository.LoadByUser(user.Id);
            return View("Notes", myNotes);
        }

        public ActionResult Download(long id)
        {
            var note = noteRepository.Load(id);
            return File(note.BinaryFile, note.FileType);
        }

        public ActionResult Edit(long id)
        {
            var note = noteRepository.Load(id);

            return View(note);
        }

        [HttpPost]
        public ActionResult Save(Note note)
        {
            note.User = userRepositoty.LoadByLogin(User.Identity.Name);
            noteRepository.Save(note);
            return RedirectToAction("PublishedNotes");
        }

        public ActionResult Details(long id)
        {
            var note = noteRepository.Load(id);
            return View(note);
        }

        [HttpPost]
        public ActionResult SearchAndSort(string title, string sortColumn)
        {
            var notes = noteRepository.LoadByTitle(title);

            switch (sortColumn)
            {
                case "title":
                    notes = notes.OrderBy(note => note.Title);
                    break;
                case "user":
                    notes = notes.OrderBy(note => note.User);
                    break;
                case "tags":
                    notes = notes.OrderBy(note => string.Join(" ", note.Tags));
                    break;
                case "date":
                    notes = notes.OrderBy(note => note.CreationDate);
                    break;
                case "public":
                    notes = notes.OrderBy(note => note.Published);
                    break;
                default:
                    break;
            }

            return PartialView("Notes", notes);
        }
    }
}