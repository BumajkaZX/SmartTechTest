namespace SmartTechTest.Game
{
    using Fight;
    using Main.Player;
    using Main.State;
    using Mobs;
    using UI;
    using UniRx;
    using Zenject;

    /// <summary>
    /// Система показа игровых функций игроку : система очков, инпут система
    /// </summary>
    public class GameController : IStateStage
    {
        [Inject]
        private IHitEvent _hitEvent;

        [Inject]
        private IGameCounter _gameCounter;

        [Inject]
        private PlayerInputView _playerInputView;

        private CompositeDisposable _disposable;
        
        public void Init()
        {
            _disposable = new CompositeDisposable();
        }

        public void Pause(bool isPaused)
        {
            
        }

        public void Start()
        {
            _hitEvent.OnHitDetected.Subscribe(go =>
            {
                if (go.TryGetComponent(out MobViewController mobView))
                {
                    OnMobHit();
                    return;
                }
                if (go.TryGetComponent(out PlayerViewController playerController))
                {
                    OnPlayerHit();
                    return;
                }
                
            }).AddTo(_disposable);
            
            _gameCounter.Enable(true);
            
            _playerInputView.Enable(true);
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                _disposable.Clear();
            }
            
            _gameCounter.Enable(false);
            _gameCounter.Clear();
            
            _playerInputView.Enable(false);
        }

        private void OnMobHit()
        {
            _gameCounter.Increase(5);
        }

        private void OnPlayerHit()
        {
            _gameCounter.Clear();
        }
    }
}
