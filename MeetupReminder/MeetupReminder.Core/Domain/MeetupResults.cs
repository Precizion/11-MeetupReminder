using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupReminder.Core.Domain
{
    public class MeetupResults
    {
        // gets and sets properties from JSON string
        public string name { get; set; }
        public long time { get; set; }
        public string link { get; set; }
    }
}
