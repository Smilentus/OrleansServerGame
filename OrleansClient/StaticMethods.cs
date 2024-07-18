using Grains.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace OrleansClient
{
    public static class StaticMethods
    {
        public static string PlayerStateToString(PlayerState? playerState)
        {
            return playerState switch
            {
                PlayerState.Draw => "Ничья",
                PlayerState.Win => "Победа",
                PlayerState.Lose => "Поражение",
                _ => "Unknown"
            };
        }

        public static ConsoleColor PlayerStateToColor(PlayerState? playerState)
        {
            return playerState switch
            {
                PlayerState.Draw => ConsoleColor.Magenta,
                PlayerState.Win => ConsoleColor.Green,
                PlayerState.Lose => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
        }

        public static T InputPrompt<T>(string prompt, params string[] waitArgs)
        {
            T output = default;

            string? input = null;
            bool isInputCorrect = false;
            do
            {
                Console.Write($"{prompt} ");
                input = Console.ReadLine();

                if (input == null) continue;

                try
                {
                    output = (T)Convert.ChangeType(input, typeof(T));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Entered value was incorrect!");
                    continue;
                }

                if (waitArgs.Length > 0)
                {
                    foreach (string arg in waitArgs)
                    {
                        if (input.Trim().ToUpper() == arg.Trim().ToUpper())
                        {
                            isInputCorrect = true;
                        }
                    }
                }
                else
                {
                    isInputCorrect = true;
                }
            }
            while (!isInputCorrect);

            return output;
        }

        public static Guid GeneratePlayerGuid(string input)
        {
            Guid playerGuid = Guid.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashed = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                playerGuid = new Guid(hashed);
            }
            return playerGuid;
        }

        public static void ColorizeOutput(string message, ConsoleColor color, bool writeLine = true)
        {
            Console.ForegroundColor = color;
            if (writeLine) Console.WriteLine(message);
            else Console.Write(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
