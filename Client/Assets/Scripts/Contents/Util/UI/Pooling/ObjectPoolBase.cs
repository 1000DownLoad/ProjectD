using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public interface IObjectPool
{
    int InitPoolSize { get; }
    int CurrentPoolSize { get; }
    int RentCount { get; }
    int MaxRentCount { get; }
    float UsePoolRatio { get; }
    float InitMaxUsePoolRatio { get; }
}

public abstract class ObjectPoolBase<T> : IDisposable, IObjectPool
{
    protected readonly Queue<T> queue = new Queue<T>();

    protected ObjectPoolBase()
    {
#if USE_POOL_MONITORING
        ObjectPoolMonitor.instance.ResistPool(this);
#endif
    }

    ~ObjectPoolBase()
    {
#if USE_POOL_MONITORING
        ObjectPoolMonitor.instance.UnresistPool(this);
#endif
    }

    static float SafeDivision(int a, int b)
    {
        if (0 == b)
        {
            return -1;
        }

        return a / (float)b;
    }

    public int InitPoolSize { get; set; }

    public virtual int CurrentPoolSize => queue.Count;

    public int RentCount { get; set; }

    public int MaxRentCount { get; set; }

    public float UsePoolRatio => SafeDivision(RentCount, CurrentPoolSize);

    public float InitMaxUsePoolRatio => SafeDivision(MaxRentCount, InitPoolSize);

    public void PreCreate(int preCreateCnt)
    {
        for (var i = 0; i < preCreateCnt; ++i)
        {
            Return(CreateInstance());
        }
        RentCount += preCreateCnt;
        InitPoolSize = preCreateCnt;
    }

    protected abstract T CreateInstance();

    public virtual T Rent(bool isInit = false)
    {
        Assert.IsFalse(null == queue);

        ++RentCount;
        MaxRentCount = Mathf.Max(MaxRentCount, RentCount);
        return queue.Count > 0 ? queue.Dequeue() : CreateInstance();
    }

    public virtual void Return(T inst)
    {
        Assert.IsFalse(null == queue);
        Assert.IsFalse(null == inst);

        --RentCount;
        queue.Enqueue(inst);
    }

    public void Dispose()
    {
        Clear();
    }

    public virtual void Clear()
    {
        queue.Clear();
    }
}

/// <summary>
/// 유니티 GameObject 풀링용 오브젝트 풀
/// </summary>
public class GameObjectPool : ObjectPoolBase<GameObject>
{
    readonly GameObject prototype;
    readonly Transform poolRoot;
    public Transform listRoot;

    public GameObjectPool(GameObject prototype, Transform poolRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = null;
    }

    public GameObjectPool(GameObject prototype, Transform poolRoot, Transform listRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = listRoot;
    }

    public List<GameObject> GetAllObjects() => queue.ToList();

    protected override GameObject CreateInstance()
    {
        var newItem = Object.Instantiate(prototype);
        newItem.transform.SetParent(poolRoot);

        return newItem;
    }

    public override GameObject Rent(bool isInit = false)
    {
        var rent = base.Rent(isInit);
        if (null == rent)
        {
            Debug.LogError("ObjectPoolBase.cs : line 131");
            return null;
        }

        if (null != listRoot)
        {
            if (rent.transform.parent != listRoot)
                rent.transform.SetParent(listRoot);

            isInit = true;
        }

        if (isInit)
        {
            rent.transform.localPosition = Vector3.zero;
            rent.transform.localScale = Vector3.one;
        }

        if (false == rent.activeSelf)
            rent.SetActive(true);

        return rent;
    }

    public override void Return(GameObject inst)
    {
        if (null != poolRoot)
            inst.transform.SetParent(poolRoot);

        if (true == inst.activeSelf)
            inst.SetActive(false);

        base.Return(inst);
    }

    public void ReturnAllList()
    {
        if (null == listRoot)
            return;

        for (int i = 0; i < listRoot.childCount; i++)
            Return(listRoot.GetChild(i).gameObject);
    }

    public override void Clear()
    {
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            if (null != cur)
                Object.Destroy(cur);
        }

        base.Clear();
    }
}

/// <summary>
/// MonoBehaviour를 상속받은 대상을 풀링하기 위한 풀
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoBehaviourObjectPool<T> : ObjectPoolBase<T> where T : MonoBehaviour
{
    protected readonly T prototype;
    readonly Transform listRoot;
    protected readonly Transform poolRoot;

    public MonoBehaviourObjectPool(T prototype, Transform poolRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = null;
    }

    public MonoBehaviourObjectPool(T prototype, Transform poolRoot, Transform listRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = listRoot;
    }

    protected override T CreateInstance()
    {
        var newItem = Object.Instantiate(prototype);
        if (null != poolRoot)
            newItem.transform.SetParent(poolRoot);

        return newItem;
    }

    void EnableComponent(Transform origin, bool enable)
    {
        Image[] lImages = origin.GetComponentsInChildren<Image>(true);
        foreach (var item in lImages)
        {
            if (item.enabled != enable)
                item.enabled = enable;
        }

        Text[] lTexts = origin.GetComponentsInChildren<Text>(true);
        foreach (var item in lTexts)
        {
            if (item.enabled != enable)
                item.enabled = enable;
        }
    }

    public override T Rent(bool isInit = false)
    {
        var rent = base.Rent(isInit);
        if (null == rent)
        {
            Debug.LogError("ObjectPoolBase.cs : line 216");
            return null;
        }

        if (null != listRoot)
        {
            if (rent.transform.parent != listRoot)
                rent.transform.SetParent(listRoot);

            isInit = true;
        }

        if (true == isInit)
        {
            rent.transform.localPosition = Vector3.zero;
            rent.transform.localScale = Vector3.one;
        }

        if (false == rent.gameObject.activeSelf)
            rent.gameObject.SetActive(true);

        return rent;
    }

    public T RentDeactive(bool isInit = false)
    {
        var rent = Rent(isInit);

        if (null != listRoot)
            EnableComponent(rent.transform, false);

        return rent;
    }

    public override void Return(T inst)
    {
        if (inst == null)
        {
            Debug.LogError("Return inst is null");
            return;
        }

        //Assert.IsFalse(ReferenceEquals(inst, prototype), $"Return Prototype Fault. : {inst.name}");
        if (null != listRoot)
        {
            EnableComponent(inst.transform, false);

            if (null != poolRoot)
                inst.transform.SetParent(poolRoot);
        }
        else
        {
            if (null != poolRoot)
                inst.transform.SetParent(poolRoot);

            if (true == inst.gameObject.activeSelf)
                inst.gameObject.SetActive(false);
        }

        base.Return(inst);
    }

    public void ReturnAllList()
    {
        if (null != listRoot)
        {
            T[] lList = listRoot.GetComponentsInChildren<T>(true);
            foreach (var item in lList)
            {
                Return(item);
            }
        }
        else if (null != poolRoot)
        {
            T[] lList = poolRoot.GetComponentsInChildren<T>(true);
            foreach (var item in lList)
            {
                Return(item);
            }
        }
    }

    public override void Clear()
    {
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            if (null != cur && null != cur.gameObject)
                Object.Destroy(cur.gameObject);
        }
        base.Clear();
    }

    public void MoveLast(T rent)
    {
        if (null != poolRoot)
            rent.transform.SetSiblingIndex(poolRoot.childCount);
    }
}


public class ObjectPool<T> : ObjectPoolBase<T> where T : Component
{
    readonly T prototype;
    readonly Transform poolRoot;
    readonly Transform listRoot;

    public ObjectPool(T prototype, Transform poolRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = null;
    }

    public ObjectPool(T prototype, Transform poolRoot, Transform listRoot)
    {
        this.prototype = prototype;
        this.poolRoot = poolRoot;
        this.listRoot = listRoot;
    }

    public List<T> GetAllObjects() => queue.ToList();

    public List<T> GetAllChildren() => listRoot.GetComponentsInChildren<T>(true).ToList();

    protected override T CreateInstance()
    {
        var newItem = Object.Instantiate(prototype);
        newItem.transform.SetParent(poolRoot);

        return newItem;
    }

    public override T Rent(bool isInit = false)
    {
        var rent = base.Rent(isInit);
        if (null == rent)
        {
            Debug.LogError("ObjectPoolBase.cs : line 374");
            return null;
        }

        if (null != listRoot)
        {
            if (rent.transform.parent != listRoot)
                rent.transform.SetParent(listRoot);

            isInit = true;
        }

        if (isInit)
        {
            rent.transform.localPosition = Vector3.zero;
            rent.transform.localScale = Vector3.one;
        }

        if (false == rent.gameObject.activeSelf)
            rent.gameObject.SetActive(true);

        return rent;
    }

    public override void Return(T inst)
    {
        if (null != poolRoot)
            inst.transform.SetParent(poolRoot);

        if (true == inst.gameObject.activeSelf)
            inst.gameObject.SetActive(false);

        base.Return(inst);
    }

    public void ReturnAllList()
    {
        if (null == listRoot)
            return;

        var list = GetAllChildren();
        foreach (var item in list)
            Return(item);
    }

    public override void Clear()
    {
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            if (null != cur)
                Object.Destroy(cur);
        }

        base.Clear();
    }
}