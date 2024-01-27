using System;
using System.Collections.Generic;

namespace Framework.Event
{
    public class EventData
    {
        public System.Type m_type;
    }

    public class GameEventHandler<T> where T : EventData
    {
        public event EventHandler<T> GameEventDataHandler;

        private readonly HashSet<System.Type> event_types = new HashSet<System.Type>();

        public void Subscribe(System.Type in_type)
        {
            event_types.Add(in_type);
        }

        public void Unsubscribe(System.Type in_type)
        {
            event_types.Remove(in_type);
        }

        public void Publish(T in_data)
        {
            if (event_types.Contains(in_data.m_type))
                GameEventDataHandler?.Invoke(this, in_data);
        }
    }
}
