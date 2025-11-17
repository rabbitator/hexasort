using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Core;
using HexaSort.Game.Level;
using HexaSort.StateMachine;

namespace HexaSort.Game.GameFlow
{
    public class LobbyState : IState, IInitializableAsync
    {
        private readonly GameConfiguration _config;
        private readonly LevelLoader _levelLoader;

        public bool Entered { get; private set; }

        public LobbyState(GameConfiguration config, LevelLoader levelLoader)
        {
            _config = config;
            _levelLoader = levelLoader;
        }

        public async UniTask InitializeAsync(CancellationToken ct) { }

        public async UniTask Enter(CancellationToken ct)
        {
            Entered = true;

            await _levelLoader.Load(_config.Levels.LobbyLevel.MapAsset);
        }

        public async UniTask Exit(CancellationToken ct)
        {
            Entered = false;

            await _levelLoader.Unload(_config.Levels.LobbyLevel.MapAsset);
        }
    }
}