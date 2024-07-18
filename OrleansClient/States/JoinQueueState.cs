using Grains.Interfaces;

namespace OrleansClient.States
{
    public class JoinQueueState : ProgramState
    {
        private IPlayersQueueGrain _queueGrain;
        private Guid? gameRoomGuid = null;

        public override async Task Enter()
        {
            _queueGrain = PlayerContext.PlayerClusterClient.GetGrain<IPlayersQueueGrain>(0);
            
            await _queueGrain.JoinQueue(PlayerContext.PlayerGuid);

            Console.WriteLine($"Joined queue as '{PlayerContext.PlayerGuid}'");
        }
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            gameRoomGuid = await PlayerContext.PlayerGrain.GetGameRoomGuid();
            
            if (gameRoomGuid == null)
            {
                Console.Write($"Players in queue ");
                StaticMethods.ColorizeOutput($"{await _queueGrain.GetQueuedPlayers()}", ConsoleColor.DarkCyan);
                Console.WriteLine($"Waiting for players to connect...");

                await Task.Delay(1000);
            }
            else
            {
                PlayerContext.GameRoomGrain = PlayerContext.PlayerClusterClient.GetGrain<IGameRoomGrain>((Guid)gameRoomGuid!);

                await ProgramStateMachine.SetState(new AcceptGameState());

                await Task.Delay(10);
            }
        }
    }
}
