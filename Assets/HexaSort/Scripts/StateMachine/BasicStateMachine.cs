using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.StateMachine
{
    public class BasicStateMachine : IStateMachine
    {
        protected IState[] _states = { };

        public void RegisterState<T>(T state) where T : IState
        {
            Array.Resize(ref _states, _states.Length + 1);
            _states[^1] = state;
        }

        public virtual async UniTask EnterState<TState>(CancellationToken ct) where TState : IState
        {
            if (ct.IsCancellationRequested) return;

            var states = _states.OfType<TState>().Where(s => !s.Entered);
            await MergedJobs(states, s => s.Enter(ct), ct);
        }

        public virtual async UniTask ExitState<TState>(CancellationToken ct) where TState : IState
        {
            if (ct.IsCancellationRequested) return;

            var states = _states.OfType<TState>().Where(s => s.Entered);
            await MergedJobs(states, s => s.Exit(ct), ct);
        }

        public virtual async UniTask ExitAllStates(CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return;

            var states = _states.Where(s => s.Entered);
            await MergedJobs(states, s => s.Exit(ct), ct);
        }

        public virtual async UniTask ExitAllExceptStates(CancellationToken ct, params IState[] states)
        {
            if (ct.IsCancellationRequested) return;

            var difference = _states.Except(states).Where(s => s.Entered);
            await MergedJobs(difference, s => s.Exit(ct), ct);
        }

        protected virtual async UniTask MergedJobs<TState>(
            IEnumerable<TState> states,
            Func<TState, UniTask> operationGetter,
            CancellationToken ct)
            where TState : IState
        {
            var tasks = Enumerable.Select(states, operationGetter);
            await UniTask.WhenAll(tasks).AttachExternalCancellation(ct).SuppressCancellationThrow();
        }
    }
}