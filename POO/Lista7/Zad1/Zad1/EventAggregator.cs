using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zad1
{
    interface ISubscriber<T>
    {
        void Handle(T Notification);
    }

    class EventAggregator
    {
        static EventAggregator _instance;
        static public EventAggregator Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new EventAggregator();
                }

                return _instance;
            }
        }

        Dictionary<Type, List<object>> _subscribers;

        public EventAggregator()
        {
            _subscribers = new Dictionary<Type, List<object>>();
        }

        public void AddSubscriber<T>(ISubscriber<T> Subscriber)
        {
            if(!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers.Add(typeof(T), new List<object>());
            }

            _subscribers[typeof(T)].Add(Subscriber);
        }

        public void RemoveSubscriber<T>(ISubscriber<T> Subscriber)
        {
            if(_subscribers.ContainsKey( typeof(T) ))
                _subscribers[typeof(T)].Remove(Subscriber);
        }

        public void Publish<T>(T Notification)
        {
            if(_subscribers.ContainsKey( typeof(T) ))
            {
                foreach(var s in _subscribers[typeof(T)])
                {
                    ((ISubscriber<T>)s).Handle(Notification);
                }
            }
        }
    }
}
