using UnityEngine;

namespace HexaSort.Core
{
    public static class AppEntry
    {
        private static bool _initialized;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Initialize()
        {
            if (_initialized) return;

            _initialized = true;

            new AppBootstrapper().Start();
        }
    }
}