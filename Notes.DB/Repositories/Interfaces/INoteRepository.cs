using System.Collections.Generic;

namespace Notes.DB.Repositories.Interfaces
{
    public interface INoteRepository : IEntityRepository<Note>
    {
        IEnumerable<Note> FindByTitle(string title);

        IEnumerable<Note> LoadAllPublished();
    }
}