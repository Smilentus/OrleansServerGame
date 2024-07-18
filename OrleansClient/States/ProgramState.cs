namespace OrleansClient.States
{
    public abstract class ProgramState
    {
        public abstract Task Enter();
        public abstract Task Exit();

        public abstract Task Loop();
    }
}
