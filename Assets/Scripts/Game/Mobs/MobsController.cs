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
        private const float MOVE_SPEED = 0.1f;
        
        private const float SHOOT_TIME = 1.2f;

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

        private float _offsetPos; //Смещение крайнего моба к левой границе

        private float _moveIterator = 0.5f; //В 0 позицию
        
        private void SpawnWave(int mobLines)
        {
           _viewControllers = _mobFactory.SpawnMob(mobLines, Random.Range(MIN_MOB_IN_LINE, MAX_MOB_IN_LINE));
           _offsetPos = _baseField.FieldBounds.min.x - _viewControllers[0].transform.position.x + _viewControllers[0].MobBounds.extents.x;
           _moveIterator = 0.5f;
           CalculateShootingMobs();
        }

        private void Update()
        {
            if (_moveIterator >= 1)
            {
                _offsetPos *= -1;
                _moveIterator = 0;
            }
            _moveIterator += MOVE_SPEED * Time.deltaTime;
                
            var currentOffset = Mathf.Lerp(-_offsetPos, _offsetPos, _moveIterator);
            
            for(int i = 0; i < _viewControllers.Count; i++)
            {
                _viewControllers[i].Move(currentOffset);
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
            
            //Движение
            Observable.EveryUpdate()
                .Where(_ => !_isPaused)
                .Subscribe(_ => Update())
                .AddTo(_disposable);

            //Стрельба
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

        //TODO: Упростить
        private void CalculateShootingMobs()
        {
            _shootingMobs = new List<MobViewController>();
            
            Dictionary<float, List<MobViewController>> table = new Dictionary<float, List<MobViewController>>();

            foreach (var viewController in _viewControllers)
            {
                if (!table.ContainsKey(viewController.transform.position.x))
                {
                    table.Add(viewController.transform.position.x, new List<MobViewController>());
                }
                
                if(table.TryGetValue(viewController.transform.position.x, out var viewControllers))
                {
                    viewControllers.Add(viewController);
                }
            }

            foreach (var posView in table)
            {
                MobViewController lowestMob = posView.Value[0];
                
                posView.Value.ForEach(controller =>
                {
                    if (controller.transform.position.y < lowestMob.transform.position.y)
                    {
                        lowestMob = controller;
                    }
                });
                
                _shootingMobs.Add(lowestMob);
            }
            
            table.Clear();
        }
        
        private void OnHit(GameObject hitObject)
        {
            if (!hitObject.TryGetComponent(out MobViewController viewController))
            {
                return;
            }

            _viewControllers.Remove(viewController);
            
            _mobFactory.ReleaseMob(viewController);
            
            CalculateShootingMobs();

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
                _shootingMobs[Random.Range(0, _shootingMobs.Count)].transform.position,
                Vector3.down * _mobGun.ProjectileSpeed, PlayerLayer);
        }
    }
}
