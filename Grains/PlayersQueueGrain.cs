using Grains.Interfaces;

namespace Grains
{
    // TODO Feature: В идеале при старте таймер запускать, который будет проверять "неактивных" игроков
    // TODO Feature: Использовать Int Key очередей для того, чтобы создавать комнаты с определённым количеством игроков.
    public class PlayersQueueGrain : Grain, IPlayersQueueGrain
    {
        private readonly List<Guid> _queuedPlayers = new(8);


        public Task<int> GetQueuedPlayers() => Task.FromResult(_queuedPlayers.Count);


        public async Task JoinQueue(Guid playerGuid)
        {
            if (_queuedPlayers.Contains(playerGuid)) return;

            _queuedPlayers.Add(playerGuid);

            Console.WriteLine($"[SERVER] Player added => {playerGuid} | Total => {_queuedPlayers.Count}");

            await CheckPairablePlayers();
        }

        public async Task LeaveQueue(Guid playerGuid)
        {
            _queuedPlayers.Remove(playerGuid);

            Console.WriteLine($"[SERVER] Player removed => {playerGuid} | Total => {_queuedPlayers.Count}");

            await Task.CompletedTask;
        }


        private async Task CheckPairablePlayers()
        {
            // TODO Feature: Где-нибудь здесь отправляем пакеты клиентам, которых выбрали для игры
            // Затем ждём от них ответа и если всё ответили, значит создаём с ними игру
            // А если через какое-то время никто не дал ответа, значит один из игроков либо вылетел, либо ещё что-то случилось
            // Следовательно неактивных игроков кикаем из очереди сразу же

            if (_queuedPlayers.Count >= 2)
            {
                await CreateGameRoom();
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        private async Task CreateGameRoom()
        {
            Console.WriteLine($"[SERVER] CreateGameRoom for 2 players");

            IGameRoomGrain gameRoomGrain = GrainFactory.GetGrain<IGameRoomGrain>(Guid.NewGuid());

            List<Guid> pairedPlayers = new List<Guid>();
            if (_queuedPlayers.Count >= 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    pairedPlayers.Add(_queuedPlayers[i]);
                    IPlayerGrain playerGrain = GrainFactory.GetGrain<IPlayerGrain>(_queuedPlayers[i]);
                    await playerGrain.SetGameRoom(gameRoomGrain.GetPrimaryKey());
                }
            }

            await gameRoomGrain.AddPlayers(pairedPlayers);

            for (int i = 0; i < 2; i++)
            {
                if (_queuedPlayers.Count > 0)
                {
                    _queuedPlayers.RemoveAt(0);
                }
            }

            Console.WriteLine($"[SERVER] Queued players => {_queuedPlayers.Count}");
        }
    }
}
