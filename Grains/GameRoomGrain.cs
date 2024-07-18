using Grains.Interfaces;

namespace Grains
{
    public class GameRoomGrain : Grain, IGameRoomGrain
    {
        // Todo: Add to room setup
        private const int MinGuess = 0;
        private const int MaxGuess = 100;

        // Todo: Add rooms with more than 2 players
        private Dictionary<Guid, GamePlayerData> _gamePlayers = null;

        private int _predictedNumberFromServer = 0;

        private GameState _gameState;


        public override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _gamePlayers = new Dictionary<Guid, GamePlayerData>(2);

            _gameState = GameState.AwaitingPlayers;

            _predictedNumberFromServer = Random.Shared.Next(MinGuess, MaxGuess);

            Console.WriteLine($"[SERVER] Server predicted the number '{_predictedNumberFromServer}'");

            return base.OnActivateAsync(cancellationToken);
        }

        public Task<GameState> GetGameState() => Task.FromResult(_gameState);

        public async Task<RuntimeGameRoomPlayerData[]> GetPlayersData()
        {
            List<RuntimeGameRoomPlayerData> output = new List<RuntimeGameRoomPlayerData>(_gamePlayers.Count);

            foreach (var kvp in _gamePlayers)
            {
                IPlayerGrain player = GrainFactory.GetGrain<IPlayerGrain>(kvp.Key);

                output.Add(new RuntimeGameRoomPlayerData(
                    await player.GetPlayerName(),
                    kvp.Value.GuessedNumber,
                    kvp.Value.GuessedDelta,
                    await player.GetPlayerWinScore(),
                    await player.GetPlayerLoseScore(),
                    kvp.Value.PlayerState
                ));
            }

            return await Task.FromResult(output.ToArray());
        }


        public Task<int> GetPredictedNumber()
        {
            if (_gameState == GameState.Finished)
                return Task.FromResult(_predictedNumberFromServer);
            else
                return Task.FromResult(-1);
        }

        public Task AddPlayers(List<Guid> players)
        {
            foreach (var player in players)
            {
                _gamePlayers[player] = new();
            }
            return Task.CompletedTask;
        }

        public async Task AcceptPlayer(Guid playerGuid)
        {
            _gamePlayers[playerGuid].IsAccepted = true;

            if (await IsAllPlayersAccepted())
            {
                _gameState = GameState.Playing;
            }
        }

        public Task<bool> IsAllPlayersAccepted()
        {
            bool isAllAccepted =
                _gamePlayers.Count(x => x.Value.IsAccepted) == _gamePlayers.Count;

            return Task.FromResult(isAllAccepted);
        }

        public async Task GuessNumber(Guid playerId, int number)
        {
            if (_gameState != GameState.Playing) return;

            if (_gamePlayers[playerId].IsPredicted) return;

            _gamePlayers[playerId].IsPredicted = true;
            _gamePlayers[playerId].GuessedNumber = number;
            _gamePlayers[playerId].GuessedDelta = Math.Abs(_predictedNumberFromServer - number);

            await CheckPlayersGuessings();
        }

        private async Task CheckPlayersGuessings()
        {
            bool isAllPlayersPredictedNumber = _gamePlayers.Count(x => x.Value.IsPredicted) == _gamePlayers.Count;

            if (isAllPlayersPredictedNumber)
            {
                await CalculateWinner();
            }
        }

        private async Task CalculateWinner()
        {
            Dictionary<int, int> distinctions = new Dictionary<int, int>();

            foreach (var kvp in _gamePlayers)
            {
                if (distinctions.ContainsKey(kvp.Value.GuessedDelta))
                {
                    distinctions[kvp.Value.GuessedDelta]++;
                }
                else
                {
                    distinctions.TryAdd(kvp.Value.GuessedDelta, 1);
                }
            }

            List<Task> tasks = new List<Task>();

            // Каким-то чудом все игроки назвали одно и то же число, значит ничья для всех
            // А если все случайно назвали одно и то же число, но это именно то число, которое загадал сервер?
            // Может тогда ничью не делать, а делать только победу? 
            if (distinctions.Count == 1)
            {
                // Draw
                foreach (var kvp in _gamePlayers)
                {
                    kvp.Value.PlayerState = PlayerState.Draw;
                }
            }
            else
            {
                // Иначе ищем минимальную дельту и для всех, кто угадал минимальную дельту ставим победу, остальным - проигрыш
                int minGuessedDelta = _gamePlayers.Min(x => x.Value.GuessedDelta);

                foreach (var kvp in _gamePlayers)
                {
                    IPlayerGrain player = GrainFactory.GetGrain<IPlayerGrain>(kvp.Key);

                    if (kvp.Value.GuessedDelta == minGuessedDelta)
                    {
                        // Win for player
                        kvp.Value.PlayerState = PlayerState.Win;
                        tasks.Add(player.AddPlayerWinScore(1));
                    }
                    else
                    {
                        // Lose for player
                        kvp.Value.PlayerState = PlayerState.Lose;
                        tasks.Add(player.AddPlayerLoseScore(1));
                    }
                }
            }

            foreach (var kvp in _gamePlayers)
            {
                IPlayerGrain player = GrainFactory.GetGrain<IPlayerGrain>(kvp.Key);

                tasks.Add(player.SetGameRoom(null));
            }

            await Task.WhenAll(tasks);

            _gameState = GameState.Finished;
        }
    }
}
