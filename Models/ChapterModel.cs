namespace Stories.Models
{
    public class ChapterModel
    {
        public int Id{ get; set; }
        public string Title{ get; set; }
        public string Body { get; set; }
        public int StoryId { get; set; }
        public StoriesModel Story {get; set; }
    }
}