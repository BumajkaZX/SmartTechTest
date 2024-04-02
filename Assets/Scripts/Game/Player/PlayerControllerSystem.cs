namespace SmartTechTest.Main.Player
{
    using Game.Field;
    using Game.Fight;
    using Game.Spawn;
    using Spawn;
    using State;
    using UniRx;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Контроллер игрока
    /// </summary>
    public class PlayerControllerSystem : IStateStage
    {
        private const string PLAYER_PATH = "Player/PlayerBase";
        
        private readonly LayerMask TargetLayer = LayerMask.NameToLayer("Enemy");

        [Inject]
        private SpawnPoints _spawnPoints;

        [Inject]
        private IPrefabSpawner _prefabSpawner;
        
        [Inject]
        private BaseField _baseField;

        [Inject]
        private PlayerControl _playerControl;

        [Inject]
        private IProjectileRequest _projectileRequest;

        private bool _isPaused;

        private PlayerViewController _playerViewController;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void SpawnPlayer()
        {
            _playerViewController =
                _prefabSpawner.Instantiate(_playerViewController);
        }

        private void ResetPlayer()
        {
            _playerViewController.transform.position = _spawnPoints.PlayerSpawnTransform.position;
            _playerViewController.gameObject.SetActive(true);
        }

        public void Init()
        {
            _playerViewController = Resources.Load<PlayerViewController>(PLAYER_PATH);
            SpawnPlayer();
            
            Observable.EveryUpdate()
                .Select(_ => _playerControl.Player.Move.ReadValue<Vector2>())
                .Where(_ => !_isPaused)
                .Subscribe(Move)
                .AddTo(_disposable);

            Observable.EveryUpdate()
                .Where(_ => _playerControl.Player.Fire.WasPressedThisFrame() && !_isPaused)
                .Subscribe(_ =>
                {
                    Fire();
                })
                .AddTo(_disposable);
        }
        
        private void Move(Vector2 dir)
        {
            var playerView = _playerViewController.PlayerView;
            playerView.UpdateView(dir);

            if (dir == Vector2.zero)
            {
                return;
            }

            var transform = _playerViewController.transform;

            Vector3 newPos =  transform.position + new Vector3(dir.x, 0, 0) * _playerViewController.MoveSpeed * Time.deltaTime;

            newPos = new Vector3(
                Mathf.Clamp(newPos.x, 
                    _baseField.FieldBounds.min.x + playerView.SpriteBounds.extents.x, 
                    _baseField.FieldBounds.max.x - playerView.SpriteBounds.extents.x),
                newPos.y, 
                newPos.z);

            transform.position = newPos;
        }
        
        private void Fire()
        {
            _projectileRequest.RequestProjectile(_playerViewController.BaseGun, _playerViewController.transform.position, Vector3.up * _playerViewController.BaseGun.ProjectileSpeed, TargetLayer);
        }

        public void Start()
        {
            ResetPlayer();
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                if (_playerViewController != null)
                {
                    Object.Destroy(_playerViewController.gameObject);
                }
                return;
            }
            
            _playerViewController.gameObject.SetActive(false);
        }

        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }
    }
}
