namespace SmartTechTest.Main.Player
{
    using Game.Field;
    using Game.Fight;
    using Game.Spawn;
    using Spawn;
    using State;
    using System;
    using UniRx;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Контроллер игрока
    /// </summary>
    public class PlayerControllerSystem : IStateStage
    {
        private const float HEIGHT_STEP = 0.7f;

        private const float MOVE_Y_DEADZONE = 0.5f;
        
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

        private int _heightStep;

        private float _moveYTime;

        private float _fireTime;

        private PlayerViewController _playerViewController;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        private void SpawnPlayer()
        {
            _playerViewController =
                _prefabSpawner.Instantiate(_playerViewController);

            _moveYTime = _playerViewController.HeightMoveTime;
            _fireTime = _playerViewController.BaseGun.ShootSpeed;
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

            Observable.EveryUpdate().Subscribe(_ => UpdateFireRate()).AddTo(_disposable);
        }
        
        private void Move(Vector2 dir)
        {
            var playerView = _playerViewController.PlayerView;
            playerView.UpdateView(dir);

            if (dir == Vector2.zero)
            {
                _moveYTime += Time.deltaTime;
                return;
            }

            var transform = _playerViewController.transform;

            Vector3 newPos =  transform.position + new Vector3(dir.x, 0, 0) * _playerViewController.MoveSpeed * Time.deltaTime;

            newPos = new Vector3(
                Mathf.Clamp(newPos.x, 
                    _baseField.FieldBounds.min.x + playerView.SpriteBounds.extents.x, 
                    _baseField.FieldBounds.max.x - playerView.SpriteBounds.extents.x),
                CalculateY(newPos.y, dir.y), 
                newPos.z);

            transform.position = newPos;
        }

        private float CalculateY(float defaultY, float inputY)
        {
            if (_moveYTime < _playerViewController.HeightMoveTime)
            {
                _moveYTime += Time.deltaTime;
                return defaultY;
            }
            
            if (inputY > MOVE_Y_DEADZONE && _heightStep < _playerViewController.MaxHeightSteps)
            {
                defaultY += HEIGHT_STEP;
                _heightStep++;
            }
            
            if(inputY < -MOVE_Y_DEADZONE && _heightStep > 0)
            {
                defaultY -= HEIGHT_STEP;
                _heightStep--;
            }

            _moveYTime = 0;

            return defaultY;
        }
        
        private void Fire()
        {
            if (_fireTime < _playerViewController.BaseGun.ShootSpeed)
            {
                return;
            }
            
            _projectileRequest.RequestProjectile(_playerViewController.BaseGun, _playerViewController.transform.position, Vector3.up * _playerViewController.BaseGun.ProjectileSpeed, TargetLayer);
            _fireTime = 0;
        }

        private void UpdateFireRate()
        {
            if (_fireTime < _playerViewController.BaseGun.ShootSpeed)
            {
                _fireTime += Time.deltaTime;
            }
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
