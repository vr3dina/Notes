using NHibernate.Criterion;
using Notes.DB.Repositories.Interfaces;
using System.Collections.Generic;

namespace Notes.DB.Repositories
{
    public class NHNoteRepository : NHBaseRepository<Note>, INoteRepository
    {
        public IEnumerable<Note> FindByTitle(string title)
        {
            var session = NHibernateHelper.GetCurrentSession();
            var notes = session.QueryOver<Note>()
                .Where(Restrictions.On<Note>(note => note.Title).IsLike($"%{title}%"))
                .And(note => note.Published == true)
                .List();

            NHibernateHelper.CloseSession();

            return notes;
        }

        //public IEnumerable<Note> FindByTag(string tags)
        //{
        //    var session = NHibernateHelper.GetCurrentSession();

        //    var notes = session.QueryOver<Note>()
        //        .Where(Restrictions.On<Note>(note => note.Tags).IsLike($"%{tags}%"))
        //        .And(note => note.Published == true)
        //        .List();

        //    NHibernateHelper.CloseSession();

        //    return notes;
        //}

        public IEnumerable<Note> LoadAllPublished()
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .Where(note => note.Published == true)
                .List();

            NHibernateHelper.CloseSession(); ;

            return notes;
        }

        public IEnumerable<Note> LoadByUser(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .Where(note => note.User.Id == userId)
                .List();

            NHibernateHelper.CloseSession(); ;

            return notes;
        }

        //public override void Save(Note entity)
        //{
            //var session = NHibernateHelper.GetCurrentSession();
            
            //try
            //{
            //    var q = session.CreateSQLQuery(
            //        $"EXEC SaveNote @Title=:title,@Published=:published,@Text=:text,@Tags=:tags," +
            //        $"@CreationDate=:date,@UserId=:userId,@BinaryFile=:fileData,@FileType=:fileType")
            //    .SetString("title", entity.Title)
            //    .SetBoolean("published", entity.Published)
            //    .SetString("text", entity.Text)
            //    .SetString("tags", entity.Tags)
            //    .SetDateTime("date", entity.CreationDate)
            //    .SetInt64("userId", entity.User.Id)
            //    .SetParameter("fileData", entity.BinaryFile)
            //    .SetString("fileType", entity.FileType)
            //    .UniqueResult();
            //}
            //finally
            //{
            //    NHibernateHelper.CloseSession();
            //}
        //}
    }
}
