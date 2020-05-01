namespace Notes.DB
{
    public class File : IEntity
    {
        public virtual long Id { get; set; }

        public virtual byte[] BinaryFile { get; set; }

        public virtual string FileType { get; set; }
    }
}
