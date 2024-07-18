namespace OrleansClient.States
{
    public static class ProgramStateMachine
    {
        private static ProgramState? _currentState = null;
        public static ProgramState? CurrentState => _currentState;


        private static bool _isRunning = true;
        public static bool IsRunning => _isRunning;


        public static void SignalToEliminate()
        {
            _isRunning = false;
        }


        public static async Task SetState(ProgramState nextState)
        {
            if (_currentState == null)
            {
                _currentState = nextState;

                await _currentState.Enter();
            }
            else
            {
                if (_currentState.Equals(nextState)) return;

                await _currentState.Exit();

                _currentState = nextState;

                await _currentState.Enter();
            }
        }

        public static async Task LoopState()
        {
            if (_currentState == null) return;

            await _currentState.Loop();
        }
    }
}
