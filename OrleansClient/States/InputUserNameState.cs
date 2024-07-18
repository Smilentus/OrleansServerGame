namespace OrleansClient.States
{
    public class InputUserNameState : ProgramState
    {
        public override Task Enter() => Task.CompletedTask;
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            string playerName = StaticMethods.InputPrompt<string>("Input player name:");

            PlayerContext.PlayerUserName = playerName;
            PlayerContext.PlayerGuid = StaticMethods.GeneratePlayerGuid(playerName);

            Console.Title = $"Orleans Game Client as {PlayerContext.PlayerUserName}";

            await ProgramStateMachine.SetState(new ConnectToServerState());
        }
    }
}
