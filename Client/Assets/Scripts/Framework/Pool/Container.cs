using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Framework.Pool
{
    //public interface IObjectPool
    //{
    //    int InitPoolSize { get; }
    //    int CurrentPoolSize { get; }
    //    int PopCount { get; }
    //    int MaxPopCount { get; }
    //    float UsePoolRatio { get; }
    //    float InitMaxUsePoolRatio { get; }
    //}


    public class Container<T> where T : class
    {
        private readonly Queue<T> _objects = new Queue<T>();

        public virtual bool Push(T obj) // return
        {
            if (obj == null)
                return false;

            _objects.Enqueue(obj);

            return true;
        }

        public virtual T Pop() // rent
        {
            if (_objects.Any() == false)
                return null;

            return _objects.Dequeue(); 
        }


        public virtual void Release()
        {
            _objects.Clear();
        }

        public Queue<T> GetObjects()
        {
            return _objects;
        }
    }

    public class GameObjectContainer : Container<GameObject>
    {
        readonly GameObject baseItem;
        readonly Transform itemParent;

        public GameObjectContainer()
        {

        }

        public GameObjectContainer(GameObject baseItem, Transform itemParent)
        {
            this.baseItem = baseItem;
            this.itemParent = itemParent;
        }

        public override GameObject Pop()
        {
            var pop = base.Pop();


            return pop;
        }

        public override bool Push(GameObject obj)
        {
            return base.Push(obj);
        }

        public override void Release() // Clear
        {
            for (int i = 0; i < itemParent.childCount; i++)
            {
                Push(itemParent.GetChild(i).gameObject);
            }
        }
    }
}