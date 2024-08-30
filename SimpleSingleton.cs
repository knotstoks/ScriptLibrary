using UnityEngine;
using Delegates;

public abstract class SimpleSingleton<T> : MonoBehaviour where T : SimpleSingleton<T>
{
    public static VoidEvent OnReady;
    public static bool IsReady => m_Instance != null && m_Instance.m_IsReady;

    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
                Debug.LogWarning("There is no instance of " + typeof(T) + " in the whole scene");
            return m_Instance;
        }
        private set => m_Instance = value;
    }

    private bool m_IsReady;

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = (T) this;  
            HandleAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void HandleAwake()
    {
        SingletonReady();
    }

    protected void SingletonReady()
    {
        m_IsReady = true;
        OnReady?.Invoke();
    }

    private void OnDestroy()
    {
        if (m_Instance == this)
        {
            HandleDestroy();
            m_Instance = null;
            m_IsReady = false;
        }
    }

    protected virtual void HandleDestroy()
    {

    }
}
