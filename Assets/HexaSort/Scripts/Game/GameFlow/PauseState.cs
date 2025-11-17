using System.Threading;
using Cysharp.Threading.Tasks;
using HexaSort.StateMachine;

namespace HexaSort.Game.GameFlow
{
    public class PauseState : IState
    {
        public bool Entered { get; private set; }

        public async UniTask Enter(CancellationToken ct)
        {
            Entered = true;
        }

        public async UniTask Exit(CancellationToken ct)
        {
            Entered = false;
        }
    }
}