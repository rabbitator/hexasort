using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Game.GameFlow;
using HexaSort.UI;
using UnityEngine;

namespace HexaSort.Core
{
    public class GameApplication : IInitializableAsync
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly GameStateMachine _gameStateMachine;
        private readonly ViewPresenter _viewPresenter;

        public GameApplication(
            GameConfiguration gameConfiguration,
            GameStateMachine gameStateMachine,
            ViewPresenter viewPresenter)
        {
            _gameConfiguration = gameConfiguration;
            _gameStateMachine = gameStateMachine;
            _viewPresenter = viewPresenter;
        }

        public async UniTask InitializeAsync(CancellationToken ct)
        {
            // TODO:
            // Register UI states in UI presenter
            // Bond game states with UI states via event bus
            // UI Presenter registers and switches the UI states
            // Game state machine controls the states and emits events,
            // which UI may or may not react to.

            SetupApplication();
            var tasks = new[] { _viewPresenter.InitializeAsync(ct) };
            await UniTask.WhenAll(tasks).AttachExternalCancellation(ct).SuppressCancellationThrow();
        }

        public async UniTask StartAsync(CancellationToken ct)
        {
            await _gameStateMachine.ExitAllStates(ct);
            await _gameStateMachine.EnterState<LobbyState>(ct);
        }

        private void SetupApplication()
        {
            Application.targetFrameRate = _gameConfiguration.Application.TargetFramerate;
        }
    }
}