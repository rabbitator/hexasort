using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.Core
{
    public interface IInitializable
    {
        void Initialize();
    }

    public interface IInitializableAsync
    {
        UniTask InitializeAsync(CancellationToken ct);
    }
}
