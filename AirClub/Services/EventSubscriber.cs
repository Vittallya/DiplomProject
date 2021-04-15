using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Services
{
    public class EventSubscriber : IDisposable
    {

        private readonly Action<EventSubscriber> action;

        public Type MesType { get; }

        public EventSubscriber(Type mesType, Action<EventSubscriber> action)
        {
            this.MesType = mesType;
            this.action = action;
        }
        public void Dispose()
        {
            action?.Invoke(this);
        }
    }
}
