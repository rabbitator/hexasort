using Cysharp.Threading.Tasks;

namespace HexaSort.GameResources
{
    public interface IResourceProvider
    {
        UniTask<IResourceHandle<T>> GetResource<T>(string key) where T : class;
    }
}