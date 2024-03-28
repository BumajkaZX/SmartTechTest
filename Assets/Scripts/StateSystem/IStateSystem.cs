namespace SmartTechTest.Main.State
{
    using UniRx;

    public interface IStateSystem
    {
        public ReactiveCommand<AppState> OnStateChange { get; }

        public void RequestState(AppState newState, bool shouldClearResources = false);

        public void RequestPause(bool isPaused);
    }
}
