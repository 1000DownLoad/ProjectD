using System;
using System.Reflection;
using UnityEngine;

namespace Framework
{
	public abstract class TMonoSingleton<TMono> : MonoBehaviour 
		where TMono : MonoBehaviour
	{
		static TMono m_Instance = null;

		static public TMono Instance
		{
			get
			{
				if (!m_Instance)
					CreateInstance();

				return m_Instance;
			}
		}

		static public GameObject MyGameObject { get; private set; } = null;
		static public Transform MyTransform { get; private set; } = null;
		static public bool IsAwaked { get; private set; }

		static TMonoSingleton<TMono> originalInst
		{
			get { return m_Instance as TMonoSingleton<TMono>; }
		}

		protected bool PreAwake()
		{
			if (m_Instance)
			{
				if (originalInst != this)
				{
					this.enabled = false;
					return false;
				}

				if (IsAwaked)
					return false;

				IsAwaked = true;
				return true;
			}

			_registerSingleton(this as TMono, false);

			IsAwaked = true;
			return true;
		}

		protected void PostAwake()
		{
		}

		protected bool PreOnDestroy()
		{
			if (originalInst != this)
				return false;

			if (!IsAwaked)
				return false;

			return true;
		}

		protected void PostOnDestroy()
		{
			if (originalInst != this)
				return;
			IsAwaked = false;
		}

		static public void CreateInstance()
		{
			if (m_Instance)
				return;

			if (typeof(TMono).IsDefined(typeof(AllowMonoSingletonByGameObjectOnlyAttribute)))
				return;

			TMono comp;
			System.Type type = typeof(TMono);

			comp = FindObjectOfType(type) as TMono;
			if (comp != null)
			{
				_registerSingleton(comp, true);
				return;
			}

			GameObject obj = new GameObject(type.ToString());
			comp = obj.AddComponent(type) as TMono;

			MyGameObject = obj;
			MyTransform = obj.transform;

			if (!m_Instance)
				_registerSingleton(comp, true);
			else
				MonoSingletonRoot.AddSingleton(originalInst);
		}
		static void _registerSingleton(TMono t, bool bNeedForcedAwake)
		{
			m_Instance = t;

			var mono = m_Instance as MonoBehaviour;
			if (mono)
			{
				MyGameObject = mono.gameObject;
				MyTransform = mono.transform;
			}

			MonoSingletonRoot.AddSingleton(originalInst);
			if (bNeedForcedAwake)
				originalInst.Awake();
		}

		protected virtual void Awake()
		{
			if (PreAwake())
			{
				PostAwake();
			}
		}
		protected virtual void OnDestroy()
		{
			if (PreOnDestroy())
			{
				PostOnDestroy();
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class AllowMonoSingletonByGameObjectOnlyAttribute : Attribute
	{
	}

	static class MonoSingletonRoot
	{
		static Lazy<GameObject> ms_lzRootGameObject = new Lazy<GameObject>(() => CreateInstanceInternal());
		static GameObject CreateInstanceInternal()
		{
			var go = new GameObject("@MonoSingletons");
			UnityEngine.Object.DontDestroyOnLoad(go);
			return go;
		}

		static public bool AddSingleton<TMono>(TMonoSingleton<TMono> inst)
			where TMono : MonoBehaviour
		{
			inst.transform.SetParent(ms_lzRootGameObject.Value.transform);
			return true;
		}
	}
}