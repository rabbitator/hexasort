using Cysharp.Threading.Tasks;
using HexaSort.Configuration;
using HexaSort.GameResources;
using UnityEngine;

namespace HexaSort.Game.Level
{
    public class LevelLoader
    {
        private readonly GameConfiguration _config;
        private readonly IResourceProvider _resourceProvider;

        public LevelLoader(GameConfiguration config, IResourceProvider resourceProvider)
        {
            _config = config;
            _resourceProvider = resourceProvider;
        }

        public async UniTask Load(int index)
        {
            var mapId = _config.Levels[index].MapAsset.AssetGUID;
            var handle = (await _resourceProvider.GetResource<HexMapAsset>(mapId));
            var map = handle.Result;
            await BuildLevel(map, $"Level_{index}");
            handle.Dispose();
        }

        private async UniTask BuildLevel(HexMapAsset mapAsset, string name)
        {
            var root = new GameObject(name);
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