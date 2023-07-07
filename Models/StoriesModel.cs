namespace Stories.Models
{
    public class StoriesModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string StroyId { get; set; }

        public List<ChapterModel> Chapters { get; set; }
    }
}