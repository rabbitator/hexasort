using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HexaSort.GameResources
{
    public class ResourceAddressableHandle<T> : IResourceHandle<T> where T : class
    {
        private AsyncOperationHandle<T> _handle;

        public UniTask<T> Task => _handle.Task.AsUniTask();
        public T Result => _handle.Result;

        public ResourceAddressableHandle(AsyncOperationHandle<T> handle)
        {
            _handle = handle;
        }

        public void Dispose()
        {
            _handle.Release();
        }
    }
}