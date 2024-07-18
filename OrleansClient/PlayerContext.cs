using Grains.Interfaces;
using Microsoft.Extensions.Hosting;

namespace OrleansClient
{
    public static class PlayerContext
    {
        public static string PlayerUserName = "";
        public static Guid PlayerGuid;

        public static IHost Host;

        public static IClusterClient PlayerClusterClient;

        public static IPlayerGrain PlayerGrain;
        public static IGameRoomGrain GameRoomGrain;
    }
}
