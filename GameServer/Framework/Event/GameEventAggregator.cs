using System;
using Framework.Event;

namespace Systems.GameEvent
{
    public static class GameEventAggregator
    {
        private static readonly EventAggregator EventAggregator = new EventAggregator();

        public static bool Subscribe<T>(Action<T> callback)
        {
            return EventAggregator.Subscribe(callback);
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            EventAggregator.Unsubscribe(callback);
        }

        public static void Publish<T>(T msg = null) where T : EventBase
        {
            EventAggregator.Publish(msg);
        }

        public static bool HasSubscribe<T>(Action<T> callback) where T : EventBase
        {
            return EventAggregator.HasSubscribe<T>(callback);
        }
    }
}
