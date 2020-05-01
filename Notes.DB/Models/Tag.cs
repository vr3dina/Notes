using System.Collections.Generic;

namespace Notes.DB
{
    public class Tag : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string TagName { get; set; }

        //public virtual IList<Note> Notes { get; set; }

    }
}
