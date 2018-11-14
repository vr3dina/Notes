using Notes.DB.Repositories.Interfaces;
using System.Collections.Generic;

namespace Notes.DB.Repositories
{
    public class NHNoteRepository : NHBaseRepository<Note>, INoteRepository
    {
        public IEnumerable<Note> LoadByTitle(string title)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .Where(note => note.Title.Contains(title))
                .List();

            NHibernateHelper.CloseSession();

            return notes;
        }

        public IEnumerable<Note> LoadAllPublished()
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .Where(note => note.Published == true)
                .List();

            NHibernateHelper.CloseSession(); ;

            return notes;
        }
    }
}
