using Grains.Interfaces;

namespace OrleansClient.States
{
    public class OnWaitingEndGameState : ProgramState
    {
        public override Task Enter() => Task.CompletedTask;
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            GameState gameState = await PlayerContext.GameRoomGrain.GetGameState();

            if (gameState == GameState.Finished)
            {
                await ProgramStateMachine.SetState(new OnReceivedGameResultsState());
            }
            else
            {
                Console.WriteLine($"Waiting other players to guess...");
                await Task.Delay(1000);
            }
        }
    }
}
