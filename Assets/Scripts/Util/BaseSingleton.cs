using UnityEngine;

namespace Util
{
    public class BaseSingleton<T>: MonoBehaviour where T: BaseSingleton<T>
    {

        private static T _instance;

        public static T Instance => _instance;
        
        public static bool Init => _instance != null;

        protected void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = (T)this;
            }
        }

        protected void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
    }
}