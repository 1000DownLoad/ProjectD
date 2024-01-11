﻿using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.Pool
{
    public class Container<T> where T : class
    {
        private readonly Queue<T> _objects = new Queue<T>();

        public bool Push(T obj) // return
        {
            if (obj == null)
                return false;

            _objects.Enqueue(obj);

            return true;
        }

        public T Pop() // rent
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