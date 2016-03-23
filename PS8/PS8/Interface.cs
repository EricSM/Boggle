using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS8
{
    interface Interface
    {
        //Simple Event
        event Action<string> TestEvent;

        //Simple String
        string Title { set; }
    }
}
