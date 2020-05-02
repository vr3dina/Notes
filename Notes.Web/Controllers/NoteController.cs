using Microsoft.Ajax.Utilities;
using Notes.DB;
using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System;
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
        private ITagRepositoty tagRepositoty;

        public NoteController()
        {
            noteRepository = new NHNoteRepository();
            userRepositoty = new NHUserRepository();
            tagRepositoty = new NHTagRepository();
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

            DB.File file = null;
            if (model.BinaryFile != null)
            {
                byte[] fileData = null;
                using (var binaryReader = new BinaryReader(model.BinaryFile.InputStream))
                {
                    fileData = binaryReader.ReadBytes(model.BinaryFile.ContentLength);
                }
                file = new DB.File()
                {
                    BinaryFile = fileData,
                    FileType = model.BinaryFile.ContentType
                };
            }

            List<Tag> tags = new List<Tag>(model.Tags.Length);
            foreach (var newTag in model.Tags)
            {
                var tag = tagRepositoty.LoadByTagName(newTag);
                tags.Add(tag ?? new Tag() { TagName = newTag });
            }


            noteRepository.Save(new Note()
            {
                Title = model.Title,
                Published = model.Published,
                Text = model.Text,
                Tags = tags.DistinctBy(t => t.TagName).ToList(),
                CreationDate = DateTime.Now,
                User = user,
                File = file
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
            return View(myNotes);
        }

        public ActionResult Download(long id)
        {
            var note = noteRepository.Load(id);
            return File(note.File.BinaryFile, note.File.FileType);
        }

        public ActionResult Edit(long id)
        {
            var note = noteRepository.Load(id);

            return View(new NoteEditModel()
            {
                Id = note.Id,
                Title = note.Title,
                Published = note.Published,
                Text = note.Text,
                Tags = note.Tags.Select(t => t.TagName).ToArray(),
                CreationDate = note.CreationDate,
                BinaryFile = note.File
            });
        }


        [HttpPost]
        public ActionResult Save(NoteEditModel note)
        {
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            List<Tag> tags = new List<Tag>(note.Tags.Length);
            foreach (var newTag in note.Tags)
            {
                var tag = tagRepositoty.LoadByTagName(newTag);
                tags.Add(tag ?? new Tag() { TagName = newTag });
            }

            noteRepository.Save(new Note()
            {
                Id = note.Id,
                Title = note.Title,
                Published = note.Published,
                Text = note.Text,
                Tags = tags.DistinctBy(t => t.TagName).ToList(),
                CreationDate = note.CreationDate,
                User = user,
                File = note.BinaryFile
            });

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
            var notes = noteRepository.FindByTitle(searchPattern);

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
                //case "tags":
                //    return notes.OrderBy(note => note.Tags);
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