using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace Classcharts_app
{
    internal class LoginResponse
    {
        public int success { get ; set; }
        public meta meta { get; set; }

    }
    internal class meta
    {
        public string session_id { get; set; }
    }
}
