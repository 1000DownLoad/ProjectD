using System;
using System.Reflection;
using UnityEngine;

namespace Framework
{
	public abstract class TMonoSingleton<TMono> : MonoBehaviour 
		where TMono : MonoBehaviour
	{
		static TMono m_Instance = null;		// 전역 인스턴스 핸들
		static bool m_onAppQuit = false;	// 프로그램이 종료될 경우의 예약 처리

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

		/// <summary>
		/// 인스턴스가 유효한가?
		/// - 인스턴스 생성과정 없이 순수하게 인스턴스가 있냐 없냐를 판단
		/// </summary>
		static public bool ValidInstance => m_Instance;

		/// <summary>
		/// 싱글턴 클래스 내부에서 자신의 인스턴스가 싱글턴이 맞는지를 확인
		/// </summary>
		static public bool IsSingleton(TMono t) { return (m_Instance && m_Instance == t); }

		static public bool IsReservedQuit() { return m_onAppQuit; }

		static TMonoSingleton<TMono> originalInst
		{
			get { return m_Instance as TMonoSingleton<TMono>; }
		}

		/// <summary>
		/// 싱글턴 클래스에 Awake() 구현 시 최상단에서 호출해줄 것.
		/// </summary>
		/// <returns>true 시 해당 싱글턴에 대한 Awake() 를 보장한다. false 시에는 Awake() 처리하지 말고 리턴시켜라</returns>
		protected bool PreAwake()
		{
			// Awake() 진입 시 이미 싱글턴이 등록되어 있는 경우
			if (m_Instance)
			{
				// 싱글턴이 이미 등록된 상태에서 다른 인스턴스가 왔을 경우는 Awake() 진행을 못하도록 무시하고 disable 시킨다.
				// Awake() 쪽에 다른 독립적 인스턴스나 자원을 로딩하는 루틴이 있다면 문제가 생길 여지가 있다.
				if (originalInst != this)
				{
					this.enabled = false;
					return false;
				}

				if (IsAwaked)   // 이미 깨어있는 상태면 더이상 진행시키지 말아라
					return false;

				IsAwaked = true;
				return true;
			}

			// 이쪽으로 빠졌다면 외부의 호출 없이 최초로 자신이 실행된 격이다.
			// 싱글턴 객체로 등록해준다.
			_registerSingleton(this as TMono, false);

			IsAwaked = true;
			return true;
		}
		/// <summary>
		/// 싱글턴 클래스에 Awake() 구현 시 최하단에서 호출해줄 것.
		/// </summary>
		protected void PostAwake()
		{
		}

		/// <summary>
		/// 싱글턴 클래스에 OnDestroy() 구현 시 최상단에서 호출해줄 것.
		/// </summary>
		/// <returns>true 시 해당 싱글턴에 대한 OnDestroy() 임을 보장한다. false 시에는 Awake() 처리하지 말고 리턴시켜라</returns>
		protected bool PreOnDestroy()
		{
			if (originalInst != this)   // 싱글턴 인스턴스가 맞아야 처리 가능
				return false;
			if (!m_onAppQuit || !IsAwaked)    // 프로그램 종료 예약이 있어야 처리 가능
				return false;
			return true;
		}
		/// <summary>
		/// 오버라이드된 OnDestroy() 최하단에서 호출해줄 것. 해당 메서드에서만 사용할 것.
		/// </summary>
		///
		protected void PostOnDestroy()
		{
			if (originalInst != this)
				return;
			IsAwaked = false;
			//m_instance = null;	// 일단 null 처리는 하지 말자. 어차피 프로그램 깨지면 모두 소멸되니까
		}

		static public void CreateInstance()
		{
			if (m_Instance || m_onAppQuit)    // 모노 싱글턴이 이미 있거나, 프로그램 종료 시점이라면 진행하지 않는다.
				return;

			//	계층에 게임오브젝트로 등록된 모노로만 싱글턴을 만드는 경우라면 여기서 리턴
			if (typeof(TMono).IsDefined(typeof(AllowMonoSingletonByGameObjectOnlyAttribute)))
				return;

			TMono comp;
			System.Type type = typeof(TMono);

			// 1차로 등록된 오브젝트를 추적해보고
			comp = FindObjectOfType(type) as TMono;
			if (comp != null)   // 이 타입의 오브젝트 인스턴스를 찾았다면 싱글턴으로 등록
			{
				_registerSingleton(comp, true);
				return;
			}

			// 등록된 오브젝트가 없다면 생성 후 등록
			GameObject obj = new GameObject(type.ToString()); // 클래스 타입명으로 빈 오브젝트를 만들고
			comp = obj.AddComponent(type) as TMono;         // 컴포넌트에 이 타입을 추가. 추가하자마자 Awake() 가 호출될 것이다.

			MyGameObject = obj;
			MyTransform = obj.transform;

			if (!m_Instance) // 여까지 왔는데 인스턴스 적재가 안되었다면 Awake() 가 호출이 안된거다.
				_registerSingleton(comp, true);
			else
				//DontDestroyOnLoad(m_Instance as MonoBehaviour);
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
			if (bNeedForcedAwake)   // 필요할 경우 강제로 Awake() 를 호출한다.
				originalInst.Awake();
		}

		/// <summary>
		/// 싱글턴 클래스에 Awake() 가 없을 경우 대신 처리
		/// </summary>
		protected virtual void Awake()
		{
			if (PreAwake()) // 맨 처음 처리할 것
			{
				// 함수를 파생시킬 경우는 이 주석 위치에 관련 프로그램을 짜면 된다.
				//
				//	programming here...

				PostAwake();    // 나갈 때 처리할 것
			}
		}
		/// <summary>
		/// 싱글턴 클래스에 OnDestroy() 가 없을 경우 대신 처리
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (PreOnDestroy()) // 맨 처음 처리할 것
			{
				// 함수를 파생시킬 경우는 이 주석 위치에 관련 프로그램을 짜면 된다.
				//
				//	programming here...

				PostOnDestroy();    // 나갈 때 처리할 것
			}
		}
		/// <summary>
		/// 싱글턴 클래스에 OnApplicationQuit() 가 없을 경우 대신 처리
		/// </summary>
		protected virtual void OnApplicationQuit()
		{
			m_onAppQuit = true;
		}
	}

	/// <summary>
	/// 계층에 등록된 게임오브젝트 기반형 모노싱글턴 사용 여부
	/// - 이 특성을 구현된 모노싱글턴 클래스에 선언 시 Awake() 를 통해 최초로 진입한 인스턴스만을 기준으로 싱글턴이 등록된다.
	/// - 모노가 없는 상태에서 Instance 호출 시 이 특성이 없으면 빈 껍데기가 만들어지지만 있다면 최초로 계층에 로딩 전까지는 null 이 된다.
	/// - 대상 게임오브젝트에 붙은 모노는 활성화 상태여야 한다. Awake() 를 통해 등록되므로...
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class AllowMonoSingletonByGameObjectOnlyAttribute : Attribute
	{
	}

	/// <summary>
	/// 모노 싱글턴들을 붙이기 위한 게임오브젝트 루트
	/// - 여기에 설정된 게임오브젝트 하위로 싱글턴 오브젝트들이 붙게 된다.
	/// - 내부 클래스이므로 바깥에서 사용은 금지한다.
	/// </summary>
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
#if UNITY_EDITOR
			if (!Application.isPlaying)
				return false;
#endif
			inst.transform.SetParent(ms_lzRootGameObject.Value.transform);
			return true;
		}
	}
}