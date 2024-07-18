namespace Grains.Interfaces
{
    public interface IPlayersQueueGrain : IGrainWithIntegerKey
    {
        public Task<int> GetQueuedPlayers();


        public Task JoinQueue(Guid playerGuid);
        public Task LeaveQueue(Guid playerGuid);
    }
}
