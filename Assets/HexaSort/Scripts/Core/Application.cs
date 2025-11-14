using Cysharp.Threading.Tasks;
using HexaSort.Analytics;
using HexaSort.Game.GameFlow;
using HexaSort.Localization;
using UnityEngine.SceneManagement;

namespace HexaSort.Core
{
    public class Application
    {
        private GameStateMachine _gameStateMachine;
        private AnalyticsReporter _analyticsReporter;
        private LocalizationProvider _localizationProvider;

        public Application(
            GameStateMachine gameStateMachine,
            AnalyticsReporter analyticsReporter,
            LocalizationProvider localizationProvider)
        {
            _gameStateMachine = gameStateMachine;
            _analyticsReporter = analyticsReporter;
            _localizationProvider = localizationProvider;
        }

        public async UniTask InitializeAsync()
        {
            var cs = new UniTaskCompletionSource();
            SceneManager.sceneLoaded += handleSceneLoaded;
            await cs.Task;

            // Proceeding initialization...

            void handleSceneLoaded(Scene scene, LoadSceneMode loadMode)
            {
                SceneManager.sceneLoaded -= handleSceneLoaded;
                cs.TrySetResult();
            }
        }

        public void Play()
        {
            //
        }
    }
}