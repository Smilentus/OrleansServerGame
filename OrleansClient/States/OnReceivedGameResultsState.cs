using Grains.Interfaces;

namespace OrleansClient.States
{
    public class OnReceivedGameResultsState : ProgramState
    {
        public override Task Enter() => Task.CompletedTask;
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            StaticMethods.ColorizeOutput(
                $"[Game results]",
                ConsoleColor.Yellow
            );

            RuntimeGameRoomPlayerData[] players = await PlayerContext.GameRoomGrain.GetPlayersData();

            StaticMethods.ColorizeOutput(
                $"\n [!] Server predicted number is '{await PlayerContext.GameRoomGrain.GetPredictedNumber()}' [!]",
                ConsoleColor.Yellow
            );

            for (int i = 0; i < players.Length; i++)
            {
                Console.Write($"\n  [{(i + 1)}] {players[i].PlayerName} is ");
                StaticMethods.ColorizeOutput(
                    $"{StaticMethods.PlayerStateToString(players[i].PlayerState)}",
                    StaticMethods.PlayerStateToColor(players[i].PlayerState), false
                );
                Console.Write($" with number '{players[i].GuessedNumber}' and delta is '{players[i].GuessedDelta}'");
            }

            StaticMethods.ColorizeOutput($"\n\n[EndGame]\n", ConsoleColor.Yellow);

            ProgramStateMachine.SignalToEliminate();
        }
    }
}
