using Notes.DB;
using Notes.DB.Repositories;
using Notes.DB.Repositories.Interfaces;
using Notes.Web.Models;
using System.Linq;
using System.Web.Mvc;

namespace Notes.Web.Controllers
{
    [Authorize]
    public class ReminderController : Controller
    {
        private IReminderRepository reminderRepository;
        private IUserRepositoty userRepositoty;

        public ReminderController()
        {
            reminderRepository = new NHReminderRepository();
            userRepositoty = new NHUserRepository();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ReminderModel model)
        {
            User user = userRepositoty.LoadByLogin(User.Identity.Name);

            reminderRepository.Save(new Reminder()
            {
                Title = model.Title,
                Description = model.Description,
                IsDone = false,
                TimeToAchieve = model.TimeToAchieve,
                User = user
            });

            return RedirectToAction("List");
        }

        public ActionResult Index()
        {
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            //var reminders = reminderRepository.LoadByUser(user.Id).OrderBy(n => n.IsDone).ThenBy(n => n.TimeToAchieve);
            var reminders = reminderRepository.LoadByUser(user.Id);
            return View(reminders);
        }

        public ActionResult List()
        {
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            var reminders = reminderRepository.LoadByUser(user.Id);
            return PartialView(reminders);
        }

        public ActionResult Edit(long id)
        {
            var reminder = reminderRepository.Load(id);
            return View(reminder);
        }


        [HttpPost]
        public ActionResult Save(Reminder reminder)
        {
            var user = userRepositoty.LoadByLogin(User.Identity.Name);
            reminder.User = user;

            reminderRepository.Save(reminder);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(long id)
        {
            reminderRepository.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateState(bool isChecked, long reminderId)
        {
            var reminder = reminderRepository.Load(reminderId);
            reminder.IsDone = isChecked;
            reminderRepository.Save(reminder);
            return RedirectToAction("List");
        }
    }
}