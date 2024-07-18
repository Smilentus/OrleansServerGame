using Grains.Interfaces;

namespace OrleansClient.States
{
    public class ActiveGameState : ProgramState
    {
        public override async Task Enter()
        {
            Console.Clear();

            StaticMethods.ColorizeOutput($"\n\n[GameRoom] Connected to game room.", ConsoleColor.Yellow);
            StaticMethods.ColorizeOutput($" Battle between this players: ", ConsoleColor.Yellow);

            RuntimeGameRoomPlayerData[] players = await PlayerContext.GameRoomGrain.GetPlayersData();

            for (int i = 0; i < players.Length; i++)
            {
                Console.Write($"  [{(i + 1)}] {players[i].PlayerName} / w:l ");
                StaticMethods.ColorizeOutput($"{players[i].WinScore}", ConsoleColor.Green, false);
                Console.Write($":");
                StaticMethods.ColorizeOutput($"{players[i].LoseScore}", ConsoleColor.Red, false);
                Console.Write("\n");
            }

            StaticMethods.ColorizeOutput($" So! The server predicted the number.", ConsoleColor.Yellow);
            StaticMethods.ColorizeOutput($"  Try to guess it!\n", ConsoleColor.Yellow);
        }
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            int yourNumber = 0;
            bool isReady = false;
            do
            {
                yourNumber = StaticMethods.InputPrompt<int>("Input your number: ");

                StaticMethods.ColorizeOutput($"Are you sure about that number? '{yourNumber}'", ConsoleColor.Yellow);

                string answer = StaticMethods.InputPrompt<string>("[Y/N]> ", "Y", "N");

                if (answer.Trim().ToUpper().Equals("Y"))
                {
                    isReady = true;
                }

            } while (!isReady);

            await PlayerContext.GameRoomGrain.GuessNumber(PlayerContext.PlayerGuid, yourNumber);

            await ProgramStateMachine.SetState(new OnWaitingEndGameState());
        }
    }
}
