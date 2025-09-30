/* ​
* Copyright (c) 2025 Wyvrn. All rights reserved. ​
* ​
*/

using UnityEngine;


namespace Interhaptics.Internal
{

    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        #region  Fields
        private static T _instance;

        private static readonly object Lock = new object();

        [SerializeField]
        private bool _persistent = true;
        #endregion

        #region  Properties
        public static bool Quitting { get; private set; }

        public static T Instance
        {
            get
            {
                if (Quitting)
                {
                    Debug.LogWarning($"[Singleton<{typeof(T)}>] Instance will not be returned because the application is quitting.");
                    return null;
                }
                lock (Lock)
                {
                    if (_instance != null)
                    {
                        return _instance;
                    }
					T[] instances;
#if UNITY_6_0_OR_NEWER
					instances = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
					instances = Object.FindObjectsOfType<T>();
#endif
					var count = instances.Length;
                    if (count == 0)
                    {
                        Debug.Log($"[Singleton<{typeof(T)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                        return _instance = new GameObject($"(Singleton){typeof(T)}").AddComponent<T>();
                    }
                    if (count > 1)
                    {
                        Debug.LogWarning($"[Singleton<{typeof(T)}>] There should never be more than one Singleton of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                        for (var i = 1; i < instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }
                    }

                    return _instance = instances[0];
                }
            }
        }
        #endregion

        #region  Methods
        private void Awake()
        {
            if (_persistent)
            {
				T[] instances;
#if UNITY_6_0_OR_NEWER
				instances = Object.FindObjectsByType<T>(FindObjectsSortMode.None);
#else
				instances = Object.FindObjectsOfType<T>();
#endif
                if (instances.Length > 1)
				{
					Destroy(gameObject);
                    return;
                }
                else
                {
                    DontDestroyOnLoad(gameObject);
                }
            }

            OnAwake();
        }

        private void OnApplicationQuit()
        {
            Quitting = true;
            OnOnApplicationQuit();
        }

        protected virtual void OnAwake() { }

        protected virtual void OnOnApplicationQuit() { }
        #endregion

    }

}