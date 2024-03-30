namespace SmartTechTest.Main.State
{
    /// <summary>
    /// Контракт всех классов в стейте
    /// </summary>
    public interface IStateStage : IPauseStage
    {
        public void Start();

        public void Stop(bool shouldClearResources);
    }
}