namespace SmartTechTest.Main.State
{
    public interface IPauseStage
    {
        public void Init();
        
        public void Pause(bool isPaused);
    }
}
