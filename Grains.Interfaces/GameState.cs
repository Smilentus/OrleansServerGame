namespace Grains.Interfaces
{
    [GenerateSerializer]
    [Serializable]
    public enum GameState
    {
        AwaitingPlayers,
        Playing,
        Finished
    }
}
