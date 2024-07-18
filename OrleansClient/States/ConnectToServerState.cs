using Grains.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace OrleansClient.States
{
    public class ConnectToServerState : ProgramState
    {
        public override async Task Enter()
        {
            await PlayerContext.Host.StartAsync();

            IClusterClient services = PlayerContext.Host.Services.GetRequiredService<IClusterClient>();

            PlayerContext.PlayerClusterClient = services;
            PlayerContext.PlayerGrain = PlayerContext.PlayerClusterClient.GetGrain<IPlayerGrain>(PlayerContext.PlayerGuid);

            Console.Clear();

            await PlayerContext.PlayerGrain.SetPlayerName(PlayerContext.PlayerUserName);
        }
        public override Task Exit() => Task.CompletedTask;


        public override async Task Loop()
        {
            await ProgramStateMachine.SetState(new ShowPlayerProfileState());
        }
    }
}
