using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace HexaSort.GameResources
{
    public class AddressableResourceProvider : IResourceProvider
    {
        public async UniTask<IResourceHandle<T>> GetResource<T>(string key) where T : class
        {
            var handle = Addressables.LoadAssetAsync<T>(key);
            var wrapHandle = new ResourceAddressableHandle<T>(handle);
            await wrapHandle.Task;

            return wrapHandle;
        }
    }
}