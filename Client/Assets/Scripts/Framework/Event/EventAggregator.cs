using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                            Debug.LogError("Callback Already Subscribed..");
                            return false;
                        }
                    }
                    _actions[typeof(T)].Add(callback);
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                return false;
            }
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            try
            {
                lock (_actions)
                {
                    if (_actions.TryGetValue(typeof(T), out var actions) == false)
                    {
                        Debug.LogError($"remove subscription error for {typeof(T).Name}, subscription not found");
                        return;
                    }

                    if (actions.Contains(callback) == false)
                    {
                        return;
                    }

                    if (actions.Remove(callback) == false)
                    {
                        Debug.LogError($"remove subscription error for {typeof(T).Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }

        public bool HasSubscribe<T>(Action<T> callback) where T : EventBase
        {
            try
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
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            return false;
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
                    try
                    {
                        if (action.Target == null)
                            continue; // the reference actually is null --> early exit

                        if ((action.Target is UnityEngine.Object) && (action.Target.Equals(null)))
                            continue;   // the object is a fake-null object --> early exit

                        action(msg);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                }
            }
            catch (Exception)
            {
                //#if UNITY_EDITOR
                //                Debug.Log(ex.ToString());
                //#endif
            }
        }
    }
}
