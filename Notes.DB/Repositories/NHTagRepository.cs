using Notes.DB.Repositories.Interfaces;

namespace Notes.DB.Repositories
{
    public class NHTagRepository : NHBaseRepository<Tag>, ITagRepositoty
    {
        public Tag LoadByTagName(string tagName)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var tag = session.QueryOver<Tag>()
                .Where(t => t.TagName == tagName)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return tag;
        }
    }
}
