using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Game.Gameplay;
using HexaSort.Game.Level;
using HexaSort.StateMachine;
using HexaSort.Storage;
using UnityEngine.AddressableAssets;

namespace HexaSort.Game.GameFlow
{
    public class GameplayState : IState
    {
        private readonly GameConfiguration _config;
        private readonly LevelLoader _levelLoader;
        private readonly ISaveDataStorage _dataStorage;
        private readonly ScoreCalculator _scoreCalculator;
        private AssetReference _levelAssetRef;

        public bool Entered { get; private set; }

        public GameplayState(GameConfiguration config, LevelLoader levelLoader, ISaveDataStorage dataStorage, ScoreCalculator scoreCalculator)
        {
            _config = config;
            _levelLoader = levelLoader;
            _dataStorage = dataStorage;
            _scoreCalculator = scoreCalculator;
        }

        public async UniTask Enter(CancellationToken ct)
        {
            Entered = true;

            var currentLevel = _dataStorage.GetInt(_config.SaveStorageKeys.CurrentLevel);
            _levelAssetRef = _config.Levels.List[currentLevel].MapAsset;
            await _levelLoader.Load(_levelAssetRef);
        }

        public async UniTask Exit(CancellationToken ct)
        {
            Entered = false;

            await _levelLoader.Unload(_levelAssetRef);
        }
    }
}