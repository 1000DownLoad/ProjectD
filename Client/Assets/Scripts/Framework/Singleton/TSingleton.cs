using System;
using System.Reflection;

namespace Framework
{
	public abstract class TSingleton<TClass>
		where TClass : class
	{
		static Lazy<TClass> m_lzInstance = new Lazy<TClass>(() => CreateInstanceInternal());
		static readonly object _LOCK = new object();    // 직접 DestroyInstance() 할 때만 사용.

		/// <summary>
		/// 싱글턴 인스턴스
		/// - 인스턴스 호출 과정은 thread-safe 하지만, 리턴 이후는 외부에서 알아서 해야한다.
		/// </summary>
		static public TClass Instance => m_lzInstance.Value;

		static public TClass Ref => m_lzInstance.Value;

		/// <summary>
		/// 인스턴스가 유효한가?
		/// - 인스턴스 생성과정 없이 순수하게 인스턴스가 있냐 없냐를 판단
		/// </summary>
		static public bool ValidInstance => m_lzInstance.IsValueCreated;

		/// <summary>
		/// 싱글턴 인스턴스 명시적 생성
		/// - 싱글턴 등록 시점에 후처리가 필요할 경우 OnCreateSingleton() 참고
		/// - 당연히... OnCreateSingleton() 내에서 Singleton.Instance 또는 DestroyInstance() 를 호출는 만행은 하지 말 것
		/// </summary>
		static public void CreateInstance()
		{
			if (m_lzInstance.IsValueCreated)
				return;

			var _ = m_lzInstance.Value;
		}

		/// <summary>
		/// 싱글턴 인스턴스 명시적 해제
		/// - 변태적인 방식인데, 런타임에서 기존 싱글턴 정보를 날리고 새로 작업해야 하는 경우 이 함수를 사용
		/// - 싱글턴 해제 시점에 후처리가 필요할 경우 OnDestroySingleton() 참고
		/// - 당연히... OnDestroySingleton() 내에서 Singleton.Instance 를 호출하는 만행은 하지 말 것
		/// </summary>
		static public void DestroyInstance()
		{
			if (!m_lzInstance.IsValueCreated)
				return;

			//	일단 락 걸고
			lock (_LOCK)
			{
				//	이중 확인
				if (m_lzInstance.IsValueCreated)
				{
					var oldInst = m_lzInstance.Value;
					DestroyInstanceInternal(oldInst);
				}
			}
		}


		#region 파생 클래스에서 구현하는 이벤트 함수

		/// <summary>
		/// 싱글턴용 인스턴스 생성됨
		/// - 아직 Instance 가 활성화되지는 않았으며 이 함수 탈출 후 targetInst 가 싱글턴 인스턴스가 된다.
		/// </summary>
		protected virtual void OnCreateSingleton() { }

		/// <summary>
		/// 싱글턴용 인스턴스 해제됨
		/// - 아직 Instance 가 활성화되지는 않았으며 이 함수 탈출 후 targetInst 가 싱글턴 인스턴스가 된다.
		/// </summary>
		protected virtual void OnDestroySingleton() { }

		static TClass CreateInstanceInternal()
		{
			var t = typeof(TClass);
			var funcName = t.ToString();

			//	생성자 조건 검사
			//	- 파생 클래스에서 비공개(protected/private) 기본 생성자가 있어야 한다.
			//	- 파생 클래스에서 공개 선언된 생성자들은 존재하면 안된다.

			var nonPublicInfo = t.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
			if (nonPublicInfo == null)
			{
				throw new Exception($"!--[{funcName}] MUST HAVE non-public default constructor.");
			}
			if (t.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length > 0)
			{
				throw new Exception($"!--[{funcName}] DO NOT HAVE any public constructors.");
			}

			TClass newInst = default;
			try
			{
				newInst = nonPublicInfo.Invoke(null) as TClass;
				//	싱글턴 인스턴스 생성 알림
				if (newInst is TSingleton<TClass> inst)
					inst.OnCreateSingleton();
			}
			catch (Exception e)
			{
				throw new Exception($"!--[{funcName}] catched some internal exceptions.", e);
			}
			finally
			{
#if SHOW_LOG_LOCAL
				if (newInst != default)
					UnityEngine.Debug.Log($"<color=#5c5c5cff>[{funcName}] Singleton Created. (TID: {System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()}).</color>");
				else
					UnityEngine.Debug.LogError($"!--[{funcName}] FAILED on creatiing. (TID: {System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()}).");
#endif
			}
			return newInst;
		}
		static void DestroyInstanceInternal(TClass oldInst)
		{
			var t = typeof(TClass);
			var funcName = t.ToString();

			try
			{
				//	싱글턴 인스턴스 해제 알림
				if (oldInst is TSingleton<TClass> inst)
					inst.OnDestroySingleton();
			}
			catch (Exception e)
			{
				throw new Exception($"!--[{funcName}] catched some internal exceptions.", e);
			}
			finally
			{
				//	새로 할당하면 지워지는 것과 마찬가지
				m_lzInstance = new Lazy<TClass>(() => CreateInstanceInternal());
#if SHOW_LOG_LOCAL
				UnityEngine.Debug.Log($"<color=#5c5c5cff>[{funcName}] Singleton Destroyed. (TID: {System.Threading.Thread.CurrentThread.ManagedThreadId.ToString()}).</color>");
#endif
			}
		}

		#endregion
	}
}
