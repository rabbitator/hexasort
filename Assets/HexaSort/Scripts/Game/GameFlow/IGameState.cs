using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.Game.GameFlow
{
    public interface IGameState : IDisposable
    {
        UniTask Initialize();
        UniTask Enter(CancellationToken ct);
        UniTask Exit(CancellationToken ct);
    }
}