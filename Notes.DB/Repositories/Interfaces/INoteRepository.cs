using System.Collections.Generic;

namespace Notes.DB.Repositories.Interfaces
{
    public interface INoteRepository : IEntityRepository<Note>
    {
        IEnumerable<Note> LoadByTitle(string title);

        IEnumerable<Note> LoadAllPublished();

        IEnumerable<Note> LoadByUser(long userId);
    }
}