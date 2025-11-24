namespace Core.Singleton
{
    public class MonoSingleton<T> : BaseBehaviour where T : BaseBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (!_instance)
                    _instance = FindFirstObjectByType<T>();
                return _instance;
            }
            set
            {
                if (_instance)
                    return;
                _instance = value;
            }
        }

        protected virtual void Awake()
        {
            _instance = GetComponent<T>();
        }
    }
}