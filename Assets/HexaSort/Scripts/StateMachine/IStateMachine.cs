using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.StateMachine
{
    public interface IStateMachine
    {
        void RegisterState<TState>(TState state) where TState : IState;

        UniTask EnterState<TState>(CancellationToken ct) where TState : IState;

        UniTask ExitState<TState>(CancellationToken ct) where TState : IState;

        UniTask ExitAllStates(CancellationToken ct);

        UniTask ExitAllExceptStates(CancellationToken ct, params IState[] states);
    }
}