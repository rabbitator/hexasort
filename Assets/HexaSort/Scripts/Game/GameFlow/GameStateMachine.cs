using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.Game.GameFlow
{
    public class GameStateMachine
    {
        private readonly LobbyState _lobbyState;
        private readonly GameplayState _gameplayState;
        private readonly PauseState _pauseState;
        private readonly List<IGameState> _states = new();

        private CancellationTokenSource _cts = new();

        public GameStateMachine(LobbyState lobbyState, GameplayState gameplayState, PauseState pauseState)
        {
            _lobbyState = lobbyState;
            _states.Add(_lobbyState);

            _gameplayState = gameplayState;
            _states.Add(_gameplayState);

            _pauseState = pauseState;
            _states.Add(_pauseState);
        }

        public async UniTask InitializeAsync()
        {
            var tasks = _states.Select(s => s.Initialize());
            await UniTask.WhenAll(tasks);
        }

        public void Start()
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
            
            _lobbyState.Enter(_cts.Token).Forget();
        }
    }
}