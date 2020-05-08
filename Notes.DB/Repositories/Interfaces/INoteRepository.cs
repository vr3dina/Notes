using System.Collections.Generic;

namespace Notes.DB.Repositories.Interfaces
{
    public interface INoteRepository : IEntityRepository<Note>
    {
        IEnumerable<Note> FindByTitle(string title);

        IEnumerable<Note> FindByTag(long tagId);

        IEnumerable<Note> LoadAllPublished();

        IEnumerable<Note> LoadByUser(long userId);
        IEnumerable<Note> LoadAllAvailable(long userId);
    }
}