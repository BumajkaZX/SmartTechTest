namespace SmartTechTest.Main.Player
{
    using Game.Fight;
    using UniRx;
    using UnityEngine;
    
    /// <summary>
    /// Контроллер для настроек и управления
    /// </summary>
    public class PlayerViewController : MonoBehaviour
    {

        public PlayerView PlayerView => _playerView;

        public float MoveSpeed => _moveSpeed;

        public float HeightMoveTime => _heightMoveTime;

        public int MaxHeightSteps => _maxHeightSteps;
        
        public BaseGun BaseGun => _bonusGun == null ? _currentGun : _bonusGun;

        [SerializeField]
        private PlayerView _playerView;

        [Header("Player settings")]

        [SerializeField, Min(0.01f)]
        private float _moveSpeed;

        [SerializeField]
        private BaseGun _currentGun;

        [SerializeField]
        private int _maxHeightSteps;

        [SerializeField]
        private float _heightMoveTime;

        private BaseGun _bonusGun;

        private float _resetTimer;

        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public void SetGun(BaseGun gun, float resetTime)
        {
            _resetTimer = 0;
            _disposable.Clear();

            _bonusGun = gun;
            
            Observable.EveryUpdate().Subscribe(_ =>
            {
                if (_resetTimer < resetTime)
                {
                    _resetTimer += Time.deltaTime;
                    return;
                }

                _bonusGun = null;
                _disposable.Clear();
            }).AddTo(_disposable);
        }

    }
}
