using HexaSort.StateMachine;

namespace HexaSort.Game.GameFlow
{
    public class GameStateMachine : BasicStateMachine
    {
        public GameStateMachine(LobbyState lobbyState, GameplayState gameplayState, PauseState pauseState)
        {
            RegisterState(lobbyState);
            RegisterState(gameplayState);
            RegisterState(pauseState);
        }
    }
}