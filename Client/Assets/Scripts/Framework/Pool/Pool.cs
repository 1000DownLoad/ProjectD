using System.Collections.Generic;

namespace Framework.Pool
{
    public class Pool<T> where T : class
    {
        private readonly Dictionary<string, Container<T>> _containers = new Dictionary<string, Container<T>>();

        public void Release()
        {
            foreach (var container in _containers)
            {
                container.Value.Release();
            }
        }

        public virtual bool Push(string key, T obj)
        {
            if (_containers.TryGetValue(key, out var container) == false)
            {
                container = new Container<T>();
                _containers.Add(key, container);
            }

            return container.Push(obj);
        }

        public virtual T Pop(string key)
        {
            if (_containers.TryGetValue(key, out var container) == false)
            {
                return null;
            }

            return container.Pop();
        }

        public Dictionary<string, Container<T>> GetContainer()
        {
            return _containers;
        }
    }

}