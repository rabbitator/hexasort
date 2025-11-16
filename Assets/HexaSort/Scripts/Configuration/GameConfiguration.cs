using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace HexaSort.Configuration
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Game/Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField]
        private ApplicationConfiguration _application;

        [SerializeField]
        private LevelConfiguration[] _levels;

        [SerializeField]
        private PrefabsConfiguration _prefabs;

        [FormerlySerializedAs("_animations")] [SerializeField]
        private List<AnimationCurvesConfiguration> animationCurves;

        [SerializeField]
        private SaveStorageKeysConfiguration _saveStorageKeys;

        public ApplicationConfiguration Application => _application;
        public IReadOnlyList<LevelConfiguration> Levels => _levels;
        public PrefabsConfiguration Prefabs => _prefabs;
        public List<AnimationCurvesConfiguration> AnimationCurves => animationCurves;
        public SaveStorageKeysConfiguration SaveStorageKeys => _saveStorageKeys;
    }

    [Serializable]
    public class ApplicationConfiguration
    {
        [SerializeField]
        private int _targetFramerate = 60;

        public int TargetFramerate => _targetFramerate;
    }

    [Serializable]
    public class LevelConfiguration
    {
        [SerializeField]
        private AssetReference _mapAsset;

        public AssetReference MapAsset => _mapAsset;
    }

    [Serializable]
    public class PrefabsConfiguration
    {
        [SerializeField]
        private AssetReference _hexGridCell;

        [SerializeField]
        private AssetReference _hexagon;

        [SerializeField]
        private AssetReference _ui;

        public AssetReference HexGridCell => _hexGridCell;
        public AssetReference Hexagon => _hexagon;
        public AssetReference UI => _ui;
    }

    [Serializable]
    public class AnimationCurvesConfiguration
    {
        public enum CurveType
        {
            SimpleBounce = 100,
            DoubleBounce = 200,
            ReturningBounce = 300,
            Shake = 400
        }

        [SerializeField]
        private CurveType _type;

        [SerializeField]
        private AnimationCurve curve = new(new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 1.0f));

        public CurveType Type => _type;
        public AnimationCurve Curve => curve;
    }

    [Serializable]
    public class SaveStorageKeysConfiguration
    {
        [SerializeField]
        private string _currentLevel = "CurrentLevel";

        public string CurrentLevel => _currentLevel;
    }
}