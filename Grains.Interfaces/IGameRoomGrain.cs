namespace Grains.Interfaces
{
    public interface IGameRoomGrain : IGrainWithGuidKey
    {
        public Task<GameState> GetGameState();

        public Task<RuntimeGameRoomPlayerData[]> GetPlayersData();

        public Task<int> GetPredictedNumber();

        public Task AddPlayers(List<Guid> players);
        
        public Task AcceptPlayer(Guid playerGuid);

        public Task<bool> IsAllPlayersAccepted();

        public Task GuessNumber(Guid playerGuid, int number);
    }
}
