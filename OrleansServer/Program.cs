using Microsoft.Extensions.Hosting;
using OrleansServer;

Console.Title = "Orleans Game Server";

using var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();

        // Это для хранения в рантайм памяти, пока сервер включен, всё хранится в оперативке, но с перезапуском данные пропадают
        siloBuilder.AddMemoryGrainStorage("PlayersStorage");

        // А это для хранения во внешней БД для постоянного хранения
        //    *Для локального теста можно закомментировать внешнюю БД и раскомментировать рантайм память
        //siloBuilder.AddAdoNetGrainStorage("PlayersStorage",
        //options =>
        //{
        //    options.Invariant = "Npgsql";
        //    options.ConnectionString = DatabaseConfigPostgreSql.ConnectionUrl;
        //});
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Server is running");
Console.WriteLine("Press 'any' key to stop and exit...");
Console.ReadKey();

await host.StopAsync();