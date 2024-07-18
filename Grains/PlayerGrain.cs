using Grains.Interfaces;
using Orleans.Providers;

namespace Grains;

[StorageProvider(ProviderName = "PlayersStorage")]
public class PlayerGrain : Grain<PlayerData>, IPlayerGrain
{
    private Guid? _gameRoomGuid = null;

    public Task SetGameRoom(Guid? gameRoomGuid)
    {
        _gameRoomGuid = gameRoomGuid;
        return Task.CompletedTask;
    }

    public Task<Guid?> GetGameRoomGuid() => Task.FromResult(_gameRoomGuid);


    public async Task SetPlayerName(string? playerName)
    {
        if (playerName == null) return;

        State.PlayerName = (string)playerName;
        await WriteStateAsync();
    }

    public Task<string> GetPlayerName() => Task.FromResult(State.PlayerName);


    public async Task AddPlayerWinScore(int? winScore)
    {
        if (winScore == null) return;

        State.WinScore += (int)winScore;
        await WriteStateAsync();
    }

    public Task<int> GetPlayerWinScore() => Task.FromResult(State.WinScore);


    public async Task AddPlayerLoseScore(int? loseScore)
    {
        if (loseScore == null) return;

        State.LoseScore += (int)loseScore;
        await WriteStateAsync();
    }

    public Task<int> GetPlayerLoseScore() => Task.FromResult(State.LoseScore);
}

[GenerateSerializer, Alias(nameof(PlayerData))]
public class PlayerData
{
    [Id(0)] public string PlayerName { get; set; } = "Unknown";
    [Id(1)] public int WinScore { get; set; } = 0;
    [Id(2)] public int LoseScore { get; set; } = 0;
}