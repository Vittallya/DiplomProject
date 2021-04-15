using AirClub.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirClub.Services
{
    public class EventBus
    {
        private ConcurrentDictionary <EventSubscriber, Func<IEvent, Task>> subscribers;

        public EventBus()
        {
            subscribers = new ConcurrentDictionary<EventSubscriber, Func<IEvent, Task>>();
        }

        public IDisposable Subscribe<T>(Func<T, Task> func) where T: IEvent
        {
            var disposableObj = new EventSubscriber(typeof(T), d => subscribers.TryRemove(d, out var _));

            subscribers.TryAdd(disposableObj, element => func((T)element));
            
            return disposableObj;
        }

        public async Task Publish<T>(T message) where T: IEvent
        {
            var messType = typeof(T);
            var tasks = subscribers
                .Where(x => x.Key.MesType == messType)
                .Select(y => y.Value(message));

            await Task.WhenAll(tasks);
        }
    }
}
