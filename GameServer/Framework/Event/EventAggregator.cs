using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Event
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<object>> _actions = new Dictionary<Type, List<object>>();
        public bool Subscribe<T>(Action<T> callback)
        {
            try
            {
                lock (_actions)
                {
                    if (_actions.ContainsKey(typeof(T)) == false)
                    {
                        _actions.Add(typeof(T), new List<object>());
                    }
                    else
                    {
                        if (_actions[typeof(T)].Contains(callback))
                        {
                            return false;
                        }
                    }
                    _actions[typeof(T)].Add(callback);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            lock (_actions)
            {
                if (_actions.TryGetValue(typeof(T), out var actions) == false)
                {
                    return;
                }

                if (actions.Contains(callback) == false)
                {
                    return;
                }

                actions.Remove(callback);
            }
        }

        public bool HasSubscribe<T>(Action<T> callback) where T : EventBase
        {
            lock (_actions)
            {
                if (_actions.TryGetValue(typeof(T), out var actions))
                {
                    return actions.Contains(callback);
                }

                return false;
            }
        }

        public void Publish<T>(T msg = null) where T : EventBase
        {
            try
            {
                var actions = _actions[typeof(T)].OfType<Action<T>>().ToList();
                if (actions.Any() == false) 
                    return;

                foreach (var action in actions)
                {
                    if (action.Target == null)
                        continue;

                    action(msg);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
