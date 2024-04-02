namespace SmartTechTest.Game.Mobs
{
    using Field;
    using Fight;
    using Main.Mob;
    using Main.Pool;
    using Main.State;
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Контроллер для управления перемещением мобов и их атаки
    /// </summary>
    public class MobsController : IStateStage, IDisposable
    {
        private const float MOVE_SPEED = 0.4f;
        
        private const float SHOOT_TIME = 2;

        private const float RESPAWN_TIME = 2;

        private const int MIN_MOB_IN_LINE = 3;

        private const int MAX_MOB_IN_LINE = 5;

        private const int MOB_LINES = 3;

        private const string MOB_GUN_PATH = "Guns/MobGun";
        
        private readonly LayerMask PlayerLayer = LayerMask.NameToLayer("Player");

        [Inject]
        private IGamePool<ParticleSystem> _deathParticles;
        
        [Inject]
        private IMobFactory _mobFactory;

        [Inject]
        private BaseField _baseField;

        [Inject]
        private IProjectileRequest _projectileRequest;

        [Inject]
        private IHitEvent _hitEvent;

        private List<MobViewController> _viewControllers;

        private List<MobViewController> _shootingMobs;

        private CompositeDisposable _disposable;

        private bool _reverseMove;

        private bool _isPaused;

        private BaseGun _mobGun;
        
        private void SpawnWave(int mobLines)
        {
           _viewControllers = _mobFactory.SpawnMob(mobLines, Random.Range(MIN_MOB_IN_LINE, MAX_MOB_IN_LINE));
           CalculateShootingMobs();
        }

        private void Update()
        {
            for (int i = 0; i < _viewControllers.Count; i++)
            {
                //Отсчитываем с разных концов
                var mob = _viewControllers[_reverseMove ? _viewControllers.Count - 1 - i : i ];

                //TODO: Возможно, добавить ограничение при последнем шаге к границам
                mob.Move((_reverseMove ? Vector2.left : Vector2.right) * MOVE_SPEED * Time.deltaTime);
                
                //левый лимит
                if (mob.transform.position.x <= _baseField.FieldBounds.min.x + mob.MobBounds.extents.x)
                {
                    _reverseMove = !_reverseMove;
                    break;
                }
                
                //правый лимит
                if (mob.transform.position.x >= _baseField.FieldBounds.max.x - mob.MobBounds.extents.x)
                {
                    _reverseMove = !_reverseMove;
                    break;
                }
            }
        }

        public void Dispose()
        {
            _mobFactory.ReleaseMob(_viewControllers);
            _disposable.Clear();
        }

        public void Init()
        {
            _disposable = new CompositeDisposable();
            _viewControllers = new List<MobViewController>();

            _mobGun = Resources.Load<BaseGun>(MOB_GUN_PATH);
        }

        public void Start()
        {
            SpawnWave(MOB_LINES);
            
            //Move
            Observable.EveryUpdate()
                .Where(_ => !_isPaused)
                .Subscribe(_ => Update())
                .AddTo(_disposable);

            //Shoot
            Observable.Timer(TimeSpan.FromSeconds(SHOOT_TIME))
                .Repeat()
                .Where(_ => !_isPaused && _viewControllers.Count != 0)
                .Subscribe(_ => Shoot())
                .AddTo(_disposable);
            
            _hitEvent.OnHitDetected.Subscribe(OnHit).AddTo(_disposable);
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                _mobFactory.ReleaseMob(_viewControllers);
                return;
            }
            
            _disposable?.Clear();
        }

        public void Pause(bool isPaused)
        {
            _isPaused = isPaused;
        }

        private void CalculateShootingMobs()
        {
            
        }
        
        private void OnHit(GameObject hitObject)
        {
            if (!hitObject.TryGetComponent(out MobViewController viewController))
            {
                return;
            }

            _viewControllers.Remove(viewController);
            
            _mobFactory.ReleaseMob(viewController);

            if (_viewControllers.Count == 0)
            {
               Observable.Timer(TimeSpan.FromSeconds(RESPAWN_TIME)).Subscribe(_ => SpawnWave(MOB_LINES));
            }
            
            var releaseCommand =  _deathParticles.RequestObject(viewController.MobConfig.DestroyParticles, out var pooledParticles);
            
            pooledParticles.transform.position = viewController.transform.position;
           
            pooledParticles.Play();

            CompositeDisposable disposable = new CompositeDisposable();
            _disposable.Add(disposable);
            
            pooledParticles.ObserveEveryValueChanged(particles => particles.isStopped)
                .Where(isStopped => isStopped)
                .Subscribe( _ =>
                {
                    releaseCommand.Execute();
                    disposable.Clear();
                    _disposable.Remove(disposable);
                }).AddTo(disposable);
        }

        private void Shoot()
        {
            _projectileRequest.RequestProjectile(_mobGun,
                _viewControllers[Random.Range(0, _viewControllers.Count)].transform.position,
                Vector3.down * _mobGun.ProjectileSpeed, PlayerLayer);
        }
    }
}
