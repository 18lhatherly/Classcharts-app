namespace Classcharts_app
{
    internal class Announcements
    {
        public List<Announcement> data {  get; set; }

    }
    internal class Announcement
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string teacher_name { get; set; }
        public string timestamp { get; set; }
        public List<Attachment> attachments { get; set; }
    }
    internal class Attachment
    {
        public string filename { get; set; }
        public string url { get; set; }
    }
}
