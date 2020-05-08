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

            noteRepository.Save(new Note()
            {
                Title = model.Title,
                Published = model.Published,
                Text = model.Text,
                Tags = GetTags(model.Tags),
                CreationDate = DateTime.Now,
                User = user,
                File = file
            });

            return RedirectToAction("Notes");
        }
        
        List<Tag> GetTags(string[] stringTags)
        {

            List<Tag> tags = new List<Tag>(stringTags.Length);
            foreach (var tagName in stringTags)
            {
                if (!string.IsNullOrWhiteSpace(tagName))
                {
                    var tag = tagRepositoty.LoadByTagName(tagName);
                    tags.Add(tag ?? new Tag() { TagName = tagName });
                }
            }
            return tags.DistinctBy(t => t.TagName).ToList();
        }

        public ActionResult Notes()
        {
            ViewBag.MyNotes = true;
            ViewBag.PNotes = false;
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            var publicNotes = noteRepository.LoadByUser(user.Id);
            return View(publicNotes);
        }

        public ActionResult List(bool showMyNotes, bool showPublishNotes)
        {
            List<Note> notes = new List<Note>();
            var user = userRepositoty.LoadByLogin(User.Identity.Name);

            if (showMyNotes && showPublishNotes)
                notes = noteRepository.LoadAllAvailable(user.Id).ToList();
            else if (showMyNotes)
                notes = noteRepository.LoadByUser(user.Id).ToList();
            else if (showPublishNotes)
                notes = noteRepository.LoadAllPublished().ToList();

            return PartialView(notes);
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

            noteRepository.Save(new Note()
            {
                Id = note.Id,
                Title = note.Title,
                Published = note.Published,
                Text = note.Text,
                Tags = GetTags(note.Tags),
                CreationDate = note.CreationDate,
                User = user,
                File = note.BinaryFile
            });

            return RedirectToAction("Notes");
        }

        public ActionResult Details(long id)
        {
            var note = noteRepository.Load(id);
            return View(note);
        }

        [HttpPost]
        public ActionResult SearchAndSort(string searchPattern, string sortOrder, string sortColumn, bool myNotes)
        {
            var notes = (myNotes)
                ? noteRepository.FindByTitle(searchPattern).Where(note => note.User.Login == User.Identity.Name)
                : noteRepository.FindByTitle(searchPattern).Where(note => note.Published);

            notes = Sort(notes, sortColumn, sortOrder);

            return PartialView("List", notes);
        }


        private IEnumerable<Note> Sort(IEnumerable<Note> notes, string sortColumn, string sortOrder)
        {
            ViewBag.SortOrder = sortOrder == "desc" ? "" : "desc";
            switch (sortColumn)
            {
                case "title":
                    return sortOrder == "desc" ? notes.OrderByDescending(n => n.Title) : notes.OrderBy(note => note.Title);
                case "user":
                    return sortOrder == "desc" ? notes.OrderByDescending(n => n.User.Login) : notes.OrderBy(note => note.User.Login);
                case "date":
                    return sortOrder == "desc" ? notes.OrderByDescending(n => n.CreationDate) : notes.OrderBy(note => note.CreationDate);
                case "public":
                    return sortOrder == "desc" ? notes.OrderByDescending(n => n.Published) : notes.OrderBy(note => note.Published);
                default:
                    return notes;
            }
        }

        public ActionResult SearchByTag(long tagId, bool myNotes)
        {
            var notes = (myNotes)
                ? noteRepository.FindByTag(tagId).Where(note => note.User.Login == User.Identity.Name)
                : noteRepository.FindByTag(tagId).Where(note => note.Published);
            return PartialView("List", notes);
        }

        public ActionResult NotesWithTag(long id)
        {
            ViewBag.MyNotes = true;
            ViewBag.PNotes = true;
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            var notes = noteRepository.FindByTag(id).Where(note => note.Published || note.User.Id == user.Id);
            return View("Notes", notes);
        }


        public ActionResult Delete(long id)
        {
            noteRepository.Delete(id);
            return RedirectToAction("Notes");
        }
    }
}