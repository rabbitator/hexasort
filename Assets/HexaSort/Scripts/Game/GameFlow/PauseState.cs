using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.Game.GameFlow
{
    public class PauseState : IGameState
    {
        public async UniTask Initialize() { }

        public void Dispose() { }

        public async UniTask Enter(CancellationToken ct) { }

        public async UniTask Exit(CancellationToken ct) { }
    }
}