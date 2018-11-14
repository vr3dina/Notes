using Notes.DB.Repositories.Interfaces;

namespace Notes.DB.Repositories
{
    public class NHUserRepository : NHBaseRepository<User>, IUserRepositoty
    {
        public User LoadByLogin(string login)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var user = session.QueryOver<User>()
                .Where(us => us.Login == login)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return user;
        }
    }
}
