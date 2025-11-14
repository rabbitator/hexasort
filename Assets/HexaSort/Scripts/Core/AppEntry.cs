using UnityEngine;

namespace HexaSort.Core
{
    public static class AppEntry
    {
        private static bool _initialized;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initialize()
        {
            if (_initialized) return;

            _initialized = true;

            new AppBootstrapper().Start();
        }
    }
}