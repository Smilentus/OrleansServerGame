using Orleans.Streams;

namespace Grains.Interfaces
{
    [GenerateSerializer]
    public class GamePlayerData
    {
        public bool IsAccepted { get; set; } = false;

        public int GuessedNumber { get; set; } = 0;
        public int GuessedDelta { get; set; } = 0;
        
        public bool IsPredicted { get; set; } = false;

        public PlayerState? PlayerState { get; set; } = null;
    }

    [GenerateSerializer]
    public record class RuntimeGameRoomPlayerData(
        string PlayerName,
        int GuessedNumber,
        int GuessedDelta,
        int WinScore,
        int LoseScore,
        PlayerState? PlayerState
    );
}