using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.Core;
using HexaSort.GameResources;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HexaSort.Game.Level
{
    public class LevelLoader : IInitializableAsync
    {
        private readonly GameConfiguration _config;
        private readonly IResourceProvider _resourceProvider;
        private readonly Dictionary<AssetReference, GameObject> _builtLevels;

        private GameObject _root;

        public LevelLoader(GameConfiguration config, IResourceProvider resourceProvider)
        {
            _config = config;
            _resourceProvider = resourceProvider;

            _builtLevels = new Dictionary<AssetReference, GameObject>();
        }

        public async UniTask InitializeAsync(CancellationToken ct)
        {
            _root = new GameObject("Levels Root");
        }

        public async UniTask Load(AssetReference assetRef)
        {
            var mapId = assetRef.AssetGUID;
            var handle = (await _resourceProvider.GetResource<HexMapAsset>(mapId));
            var map = handle.Result;
            var levelRoot = await BuildLevel(map, "Level");
            handle.Dispose();

            _builtLevels[assetRef] = levelRoot;
        }

        public async UniTask Unload(AssetReference mapAsset)
        {
            if (_builtLevels.TryGetValue(mapAsset, out var root))
            {
                Object.Destroy(root);
                _builtLevels[mapAsset] = null;
            }
        }

        private async UniTask<GameObject> BuildLevel(HexMapAsset mapAsset, string name)
        {
            var root = new GameObject($"{name}");
            root.transform.SetParent(_root.transform);

            var cellId = _config.Prefabs.HexGridCell.AssetGUID;
            var handle = (await _resourceProvider.GetResource<GameObject>(cellId));
            var cellPrefab = handle.Result;

            for (var y = 0; y < mapAsset.size.y; y++)
            {
                for (var x = 0; x < mapAsset.size.x; x++)
                {
                    var index = y * mapAsset.size.x + x;

                    PlaceCell(x, y, index, root, mapAsset, cellPrefab);
                }
            }

            handle.Dispose();

            return root;
        }

        private void PlaceCell(int x, int y, int index, GameObject root, HexMapAsset mapAsset, GameObject cellPrefab)
        {
            if (!mapAsset.cells[index]) return;

            var cell = Object.Instantiate(cellPrefab, root.transform);
            var xPos = x * 0.8667f + (y % 2 == 0 ? 0.0f : 0.43333f);
            var yPos = -y * 0.75f;
            cell.transform.localPosition = new Vector3(xPos, 0.02f /* TODO: Take it to config */, yPos);
        }
    }
}