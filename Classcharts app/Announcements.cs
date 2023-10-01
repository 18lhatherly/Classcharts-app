using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
