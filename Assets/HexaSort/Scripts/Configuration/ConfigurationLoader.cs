using Cysharp.Threading.Tasks;

namespace HexaSort.Configuration
{
    public class ConfigurationLoader
    {
        public async UniTask<T> LoadAsync<T>() where T : class, new()
        {
            return new T();
        }
    }
}