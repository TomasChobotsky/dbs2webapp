namespace dbs2webapp.Entities
{
    public class ChapterImage
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long FileSize { get; set; }
        public byte[]? Data { get; set; }

        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
    }
}
