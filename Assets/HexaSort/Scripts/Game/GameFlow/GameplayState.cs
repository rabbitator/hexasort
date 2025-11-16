using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Game.Gameplay;
using HexaSort.Game.Level;
using HexaSort.Storage;

namespace HexaSort.Game.GameFlow
{
    public class GameplayState : IGameState
    {
        private readonly GameConfiguration _config;
        private readonly LevelLoader _levelLoader;
        private readonly ISaveDataStorage _dataStorage;
        private readonly ScoreCalculator _scoreCalculator;

        public GameplayState(GameConfiguration config, LevelLoader levelLoader, ISaveDataStorage dataStorage, ScoreCalculator scoreCalculator)
        {
            _config = config;
            _levelLoader = levelLoader;
            _dataStorage = dataStorage;
            _scoreCalculator = scoreCalculator;
        }

        public async UniTask Initialize()
        {
            var currentLevel = _dataStorage.GetInt(_config.SaveStorageKeys.CurrentLevel);
            await _levelLoader.Load(currentLevel);
        }

        public void Dispose() { }

        public async UniTask Enter(CancellationToken ct) { }

        public async UniTask Exit(CancellationToken ct) { }
    }
}