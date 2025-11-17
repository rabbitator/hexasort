using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.Core;
using HexaSort.GameResources;

namespace HexaSort.Configuration
{
    public class ConfigurationLoader : IInitializableAsync
    {
        private readonly IResourceProvider _resourceProvider;

        private const string ConfigName = "HexaSortConfig";

        public GameConfiguration Config { get; private set; }

        public ConfigurationLoader(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public async UniTask InitializeAsync(CancellationToken ct)
        {
            var handle = await _resourceProvider.GetResource<GameConfiguration>(ConfigName);
            Config = handle.Result;
        }
    }
}