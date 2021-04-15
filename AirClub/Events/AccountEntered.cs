using System;
using System.Collections.Generic;
using System.Text;
using AirClub.Model;

namespace AirClub.Events
{
    public class AccountEntered : IEvent
    {
        public AccountEntered(Employee acc)
        {
            Account = acc;
        }

        public Employee Account { get; }
    }
}
