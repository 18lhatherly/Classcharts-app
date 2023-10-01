using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Classcharts_app
{
    internal class Email
    {
        public Sender sender {  get; set; }
        public Sender to { get; set; }
        public string subject { get; set; }
        public string htmlContent { get; set; }

    }
    internal class Sender
    {
        public Sender(string name, string email)
        {
            this.name = name;
            this.email = email;
        }

        public string name { get; set; }
        public string email { get; set; }

    }
}
