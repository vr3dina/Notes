using Notes.DB.Repositories.Interfaces;
using System.Collections.Generic;

namespace Notes.DB.Repositories.Interfaces
{
    public interface IReminderRepository : IEntityRepository<Reminder>
    {
        IEnumerable<Reminder> LoadByUser(long userId);
    }
}