namespace OrleansClient.States
{
    public class AcceptGameState : ProgramState
    {
        public override Task Enter() => Task.CompletedTask;
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            await PlayerContext.GameRoomGrain.AcceptPlayer(PlayerContext.PlayerGuid);

            while (await PlayerContext.GameRoomGrain.IsAllPlayersAccepted() == false)
            {
                Console.WriteLine($"Waiting for players to accept...");

                await Task.Delay(1000);
            }

            await ProgramStateMachine.SetState(new ActiveGameState());
        }
    }
}
