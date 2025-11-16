using System;
using Cysharp.Threading.Tasks;

namespace HexaSort.GameResources
{
    public interface IResourceHandle<T> : IDisposable
    {
        public UniTask<T> Task { get; }
        public T Result { get; }
    }
}