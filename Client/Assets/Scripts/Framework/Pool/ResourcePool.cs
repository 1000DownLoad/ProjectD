using System.Collections.Generic;
using UnityEngine;

namespace Framework.Pool
{
    public class ResourcePool
    {
        public readonly Pool<GameObject> Pool = new Pool<GameObject>();

        public GameObject ResourceRoot;

        // 생성된 게임 오브젝트
        public List<GameObject> total_objects = new List<GameObject>();

        public void Initialize(GameObject resourceRoot)
        {
            ResourceRoot = resourceRoot;
        }

        public void Release()
        {
            foreach (var go in total_objects)
            {
                Object.DestroyImmediate(go);
            }

            total_objects.Clear();
            Pool.Release();
        }

        public void PreInstantiate(GameObject gameObject, int count)
        {
            if (ResourceRoot == null)
                return;

            for (int i = 0; i < count; ++i)
            {
                var go          = GameObject.Instantiate(gameObject, ResourceRoot.transform);
                var cloneRemove = go.name.Replace("(Clone)", "");
                go.name = cloneRemove;

                go.gameObject.SetActive(false);
                Pool.Push(go.name, go);

                total_objects.Add(go);
            }
        }

        public GameObject Pop(string name)
        {
            var go = Pool.Pop(name);
            if (go == null)
                return null;

            if (go.activeSelf)
                return null;

            go.SetActive(true);

            return go;
        }
        public void Push(GameObject go)
        {
            go.SetActive(false);

            Pool.Push(go.name, go);
        }

        public GameObject PopWithPreInstantiate(string name, GameObject resource)
        {
            var go = Pop(name);
            if (go == null)
            {
                PreInstantiate(resource, 1);
                go = Pop(name);
            }

            if (go == null)
                return null;

            go.SetActive(true);

            return go;
        }
    }
}