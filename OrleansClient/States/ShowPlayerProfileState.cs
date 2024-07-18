namespace OrleansClient.States
{
    public class ShowPlayerProfileState : ProgramState
    {
        public override Task Enter() => Task.CompletedTask;
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            Console.WriteLine($"Your profile name is '{await PlayerContext.PlayerGrain.GetPlayerName()}'");
            Console.Write($"Your game scores [win:lose] => [");
            StaticMethods.ColorizeOutput($"{await PlayerContext.PlayerGrain.GetPlayerWinScore()}", ConsoleColor.Green, false);
            Console.Write($":");
            StaticMethods.ColorizeOutput($"{await PlayerContext.PlayerGrain.GetPlayerLoseScore()}", ConsoleColor.Red, false);
            Console.Write("]\n");

            await ProgramStateMachine.SetState(new JoinQueueState());
        }
    }
}
