namespace SmartTechTest.Game.Mobs
{
    using Field;
    using Fight;
    using Main.Mob;
    using Main.State;
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Контроллер для управления перемещением мобов и их атаки
    /// </summary>
    public class MobsController : IStateStage, IDisposable
    {
        private const float MOVE_SPEED = 0.4f;
        
        private const float SHOOT_TIME = 2;

        private const int MIN_MOB_IN_LINE = 3;

        private const int MAX_MOB_IN_LINE = 5;

        private const string MOB_GUN_PATH = "Guns/MobGun";

        [Inject]
        private IMobFactory _mobFactory;

        [Inject]
        private BaseField _baseField;

        [Inject]
        private IProjectileRequest _projectileRequest;

        private List<MobViewController> _viewControllers;

        private List<MobViewController> _shootingMobs;

        private CompositeDisposable _disposable;

        private bool _reverseMove;

        private bool _isPaused;

        private BaseGun _mobGun;
        
        public void SpawnWave(int mobLines)
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
            SpawnWave(3);
            
            //Move
            Observable.EveryUpdate()
                .Where(_ => !_isPaused)
                .Subscribe(_ => Update())
                .AddTo(_disposable);

            //Shoot
            Observable.Timer(TimeSpan.FromSeconds(SHOOT_TIME))
                .Repeat()
                .Where(_ => !_isPaused)
                .Subscribe(_ => Shoot())
                .AddTo(_disposable);
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                foreach (var controller in _viewControllers)
                {
                    Object.Destroy(controller.gameObject);
                }
                
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

        private void Shoot()
        {
            _projectileRequest.RequestProjectile(_mobGun, _viewControllers[Random.Range(0, _viewControllers.Count)].transform.position, Vector3.down * _mobGun.ProjectileSpeed);
        }
    }
}
