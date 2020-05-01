namespace Notes.DB.Repositories.Interfaces
{
    public interface ITagRepositoty : IEntityRepository<Tag>
    {
        Tag LoadByTagName(string tag);
    }
}
