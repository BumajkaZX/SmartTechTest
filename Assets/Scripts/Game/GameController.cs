namespace SmartTechTest.Game
{
    using Fight;
    using Main.Player;
    using Main.State;
    using Mobs;
    using UI;
    using UniRx;
    using Zenject;

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
                    OnMobHitted();
                    return;
                }
                if (go.TryGetComponent(out PlayerViewController playerController))
                {
                    OnPlayerHitted();
                    return;
                }
                
            }).AddTo(_disposable);
            
            _gameCounter.Enable(true);
            
            _playerInputView.gameObject.SetActive(true);
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                _disposable.Clear();
            }
            
            _gameCounter.Enable(false);
            _gameCounter.Clear();
            
            _playerInputView.gameObject.SetActive(false);
        }

        private void OnMobHitted()
        {
            _gameCounter.Increase(5);
        }

        private void OnPlayerHitted()
        {
            
        }
    }
}
