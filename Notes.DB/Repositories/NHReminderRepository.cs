using Notes.DB.Repositories.Interfaces;
using System.Collections.Generic;

namespace Notes.DB.Repositories
{
    public class NHReminderRepository : NHBaseRepository<Reminder>, IReminderRepository
    {
        public IEnumerable<Reminder> LoadByUser(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var reminders = session.QueryOver<Reminder>()
                .Where(r => r.User.Id == userId)
                .List();

            NHibernateHelper.CloseSession(); ;

            return reminders;
        }
    }
}
