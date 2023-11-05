namespace Classcharts_app
{
    internal class SentAnnouncements
    {
        private readonly string filename = "./sent-announcements.txt";
        private List<string> sentAnnouncements = new List<string>();

        public SentAnnouncements()
        {
            if (File.Exists(filename))
            {
                string[] lines = File.ReadAllLines(filename);
                sentAnnouncements.AddRange(lines);
            }
        }

        public bool alreadySent(Announcement announcement)
        {
            if (sentAnnouncements.Contains(key(announcement)))
            {
                return true;
            }
            return false;
        }

        public void addToSent(Announcement announcement)
        {
            sentAnnouncements.Add(key(announcement));
        }

        private string key(Announcement announcement)
        {
            return announcement.id + "-" + announcement.timestamp;
        }

        public void StoreSent()
        {
            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                foreach (string line in sentAnnouncements)
                    outputFile.WriteLine(line);
            }
        }
    }
}
