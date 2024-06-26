namespace SmartTechTest.Main.State
{
    using UniRx;
    using Zenject;

    /// <summary>
    /// Стейтмашина приложения
    /// </summary>
    public class AppStateSystem : IStateSystem
    {
        public ReactiveCommand<AppState> OnStateChange { get; } = new ReactiveCommand<AppState>();

        private AppState _currentState;

        [Inject]
        private DiContainer _container;
        
        public void RequestState(AppState newState, bool shouldClearResources = false)
        {
            if (_currentState != null)
            {
                _currentState.Exit(shouldClearResources);
            }
            
            _currentState = newState;
            _currentState.Init(_container);
            OnStateChange.Execute(_currentState);
            _currentState.Enter();
        }

        public void RequestPause(bool isPaused)
        {
            if (_currentState == null)
            {
                return;
            }

            _currentState.RequestPause(isPaused);
        }
    }
}