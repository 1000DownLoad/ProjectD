public class TSingleton<T> where T : class, new()
{
    private static T m_instance;
    private static readonly object lockObject = new object();

    protected TSingleton()
    {
        OnCreateSingleton();
    }

    protected virtual void OnCreateSingleton() { }

    public static T Instance {
        get {
            if (m_instance == null)
            {
                // 두 개 이상의 스레드가 동시에 인스턴스를 생성하지 못하도록 lock을 사용합니다.
                lock (lockObject)
                {
                    if (m_instance == null)
                    {
                        m_instance = new T();
                    }
                }
            }

            return m_instance;
        }
    }
}