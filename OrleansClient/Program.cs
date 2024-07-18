using Microsoft.Extensions.Hosting;
using OrleansClient;
using OrleansClient.States;

Console.Title = "Orleans Game Client";

using IHost host = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering();
    })
    .UseConsoleLifetime()
    .Build();

PlayerContext.Host = host;

await MainLoop();

static async Task MainLoop()
{
    await ProgramStateMachine.SetState(new InputUserNameState());

    while (ProgramStateMachine.IsRunning)
    {
        await ProgramStateMachine.LoopState();

        await Task.Delay(40);
    }

    await PlayerContext.Host.StopAsync();
}