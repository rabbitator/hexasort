using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.StateMachine
{
    public interface IState
    {
        bool Entered { get; }

        UniTask Enter(CancellationToken ct);

        UniTask Exit(CancellationToken ct);
    }
}