using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework.Pool
{
    public class Container<T> where T : class
    {
        public Container() { }

        public Container(T baseItem, Transform itemParent)
        {

        }

        private readonly Queue<T> _objects = new Queue<T>();

        public bool Push(T obj) // Return
        {
            if (obj == null)
                return false;

            _objects.Enqueue(obj);

            return true;
        }

        public T Pop() // Rent 
        {
            if (_objects.Any() == false)
                return null;

            return _objects.Dequeue(); 
        }

        public void Release()
        {
            _objects.Clear();
        }

        public Queue<T> GetObjects()
        {
            return _objects;
        }
    }
}