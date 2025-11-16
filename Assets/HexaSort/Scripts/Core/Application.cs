using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Game.GameFlow;
using HexaSort.UI;
using UnityEngine.SceneManagement;

namespace HexaSort.Core
{
    public class Application
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ViewPresenter _viewPresenter;

        public Application(
            GameConfiguration gameConfiguration,
            GameStateMachine gameStateMachine,
            ViewPresenter viewPresenter)
        {
            _gameConfiguration = gameConfiguration;
            _gameStateMachine = gameStateMachine;
            _viewPresenter = viewPresenter;
        }

        public async UniTask InitializeAsync()
        {
            SetupApplication();

            await WaitSceneLoad();
            var t1 = _gameStateMachine.InitializeAsync();
            var t2 = _viewPresenter.InitializeAsync();

            await UniTask.WhenAll(t1, t2);
        }

        public void Play()
        {
            _gameStateMachine.Start();
        }

        private void SetupApplication()
        {
            UnityEngine.Application.targetFrameRate = _gameConfiguration.Application.TargetFramerate;
        }

        private async UniTask WaitSceneLoad()
        {
            if (SceneManager.loadedSceneCount > 0) return;

            var cs = new UniTaskCompletionSource();
            SceneManager.sceneLoaded += handleSceneLoaded;
            await cs.Task;

            void handleSceneLoaded(Scene scene, LoadSceneMode loadMode)
            {
                SceneManager.sceneLoaded -= handleSceneLoaded;
                cs.TrySetResult();
            }
        }
    }
}