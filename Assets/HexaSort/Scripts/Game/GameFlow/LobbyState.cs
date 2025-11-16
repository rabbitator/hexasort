using System.Threading;
using Cysharp.Threading.Tasks;

namespace HexaSort.Game.GameFlow
{
    public class LobbyState : IGameState
    {
        public async UniTask Initialize() { }

        public void Dispose() { }

        public async UniTask Enter(CancellationToken ct) { }

        public async UniTask Exit(CancellationToken ct) { }
    }
}