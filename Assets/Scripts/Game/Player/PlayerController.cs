namespace SmartTechTest.Main.Player
{
    using Game.Field;
    using Game.Fight;
    using UniRx;
    using UnityEngine;
    using Zenject;
    
    //TODO: Вынести всё управление в отдельную систему
    /// <summary>
    /// Контроллер игрока 
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        
        [Inject]
        private PlayerControl _playerControl;

        [Inject]
        private BaseField _baseField;

        [Inject]
        private IProjectileRequest _projectileRequest;

        [SerializeField]
        private PlayerView _playerView;

        [Header("Player settings")]

        [SerializeField, Min(0.01f)]
        private float _moveSpeed;

        [SerializeField]
        private BaseGun _currentGun;
        
        private LayerMask _targetLayer;

        private float _fireSpeed;

        private CompositeDisposable _disposable = new CompositeDisposable();

        private void Awake()
        {
            _fireSpeed = _currentGun.ShootSpeed;

            _targetLayer = LayerMask.NameToLayer("Enemy");
            
            Observable.EveryUpdate()
                .Select(_ => _playerControl.Player.Move.ReadValue<Vector2>())
                .Subscribe(Move)
                .AddTo(_disposable);

            Observable.EveryUpdate()
                .Where(_ => _playerControl.Player.Fire.WasPressedThisFrame())
                .Subscribe(_ =>
                {
                    Fire();
                })
                .AddTo(_disposable);
        }

        //TODO: при изменении поля в рантайме апдейтить позицию
        private void Move(Vector2 dir)
        {
            _playerView.UpdateView(dir);

            if (dir == Vector2.zero)
            {
                return;
            }

            Vector3 newPos =  transform.position + new Vector3(dir.x, 0, 0) * _moveSpeed * Time.deltaTime;

            newPos = new Vector3(
                Mathf.Clamp(newPos.x, 
                    _baseField.FieldBounds.min.x + _playerView.SpriteBounds.extents.x, 
                    _baseField.FieldBounds.max.x - _playerView.SpriteBounds.extents.x),
                newPos.y, 
                newPos.z);

            transform.position = newPos;
        }
        
        private void Fire()
        {
            _projectileRequest.RequestProjectile(_currentGun, transform.position, Vector3.up * _currentGun.ProjectileSpeed, _targetLayer);
        }
        
        private void OnDestroy()
        {
            _disposable.Clear();
        }
    }
}
