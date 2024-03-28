namespace SmartTechTest.Main.State
{
    /// <summary>
    /// Контракт всех классов в стейте
    /// </summary>
    public interface IStateStage
    {
        public void Init();

        public void Start();

        public void Stop(bool shouldClearResources);

        public void Pause(bool isPaused);
    }
}