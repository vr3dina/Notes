using Notes.DB;
using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
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
                Tags = string.Join(" ", model.Tags),
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
        public ActionResult SearchAndSort(string searchPattern, string searchField, string sortColumn)
        {
            var notes = (searchField == "tags") ? noteRepository.FindByTag(searchPattern) : noteRepository.FindByTitle(searchPattern);

            notes = Sort(notes, sortColumn);

            return PartialView("Notes", notes);
        }


        private IEnumerable<Note> Sort(IEnumerable<Note> notes, string sortColumn)
        {
            switch (sortColumn)
            {
                case "title":
                    return notes.OrderBy(note => note.Title);
                case "user":
                    return notes.OrderBy(note => note.User.Login);
                case "tags":
                    return notes.OrderBy(note => note.Tags);
                case "date":
                    return notes.OrderBy(note => note.CreationDate);
                case "public":
                    return notes.OrderBy(note => note.Published);
                default:
                    break;
            }
            return notes;
        }


        public ActionResult Delete(long id)
        {
            noteRepository.Delete(id);
            return RedirectToAction("MyNotes");
        }
    }
}