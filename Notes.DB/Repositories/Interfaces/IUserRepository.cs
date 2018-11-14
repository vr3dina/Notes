namespace Notes.DB.Repositories.Interfaces
{
    public interface IUserRepositoty : IEntityRepository<User>
    {
        User LoadByLogin(string login);
    }
}
