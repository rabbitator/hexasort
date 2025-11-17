using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Game.GameFlow;
using HexaSort.UI;

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
            await UniTask.WhenAll(
                _gameStateMachine.InitializeAsync(),
                _viewPresenter.InitializeAsync());
        }

        public void Play()
        {
            _gameStateMachine.Start();
        }

        private void SetupApplication()
        {
            UnityEngine.Application.targetFrameRate = _gameConfiguration.Application.TargetFramerate;
        }
    }
}