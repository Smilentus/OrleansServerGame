namespace Grains.Interfaces;

public interface IPlayerGrain : IGrainWithGuidKey
{
    public Task SetGameRoom(Guid? gameRoomGuid);
    public Task<Guid?> GetGameRoomGuid();

    public Task SetPlayerName(string? playerName);
    public Task<string> GetPlayerName();

    public Task AddPlayerWinScore(int? winScore);
    public Task<int> GetPlayerWinScore();

    public Task AddPlayerLoseScore(int? loseScore);
    public Task<int> GetPlayerLoseScore();
}