using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace AirClub.Events
{
    public class ChangePage: IEvent
    {
        public Page Page { get; }
        public ChangePage(Page page)
        {
            Page = page;
        }
    }
}
