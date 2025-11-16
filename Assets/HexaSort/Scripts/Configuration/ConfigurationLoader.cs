using Cysharp.Threading.Tasks;
using HexaSort.GameResources;

namespace HexaSort.Configuration
{
    public class ConfigurationLoader
    {
        private readonly IResourceProvider _resourceProvider;

        public ConfigurationLoader(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public async UniTask<GameConfiguration> LoadAsync()
        {
            var handle = await _resourceProvider.GetResource<GameConfiguration>("HexaSortConfig");

            return handle.Result;
        }
    }
}