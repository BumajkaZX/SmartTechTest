namespace SmartTechTest.Game.Mobs
{
    using Field;
    using Main.Mob;
    using Main.Pool;
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

        private const int MIN_MOB_IN_LINE = 3;

        private const int MAX_MOB_IN_LINE = 5;
        
        [Inject]
        private IGamePool<ParticleSystem> _destroyPool;

        [Inject]
        private IMobFactory _mobFactory;

        [Inject]
        private BaseField _baseField;

        private List<MobViewController> _viewControllers;

        private List<MobViewController> _shootingMobs;

        private CompositeDisposable _disposable;

        private bool _reverseMove;

        public MobsController()
        {
            _disposable = new CompositeDisposable();
            _viewControllers = new List<MobViewController>();
        }
        
        public void SpawnWave(int mobLines)
        {
           _viewControllers = _mobFactory.SpawnMob(mobLines, Random.Range(MIN_MOB_IN_LINE, MAX_MOB_IN_LINE));
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
            
        }

        public void Start()
        {
            SpawnWave(3);
            
            StartMove();
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
        }

        public void Pause(bool isPaused)
        {
            if (isPaused)
            {
                _disposable.Clear();
                return;
            }
            
            StartMove();
        }

        private void CalculateShootingMobs()
        {
            
        }

        private void StartMove()
        {
            Observable.EveryUpdate()
                .Subscribe(_ => Update())
                .AddTo(_disposable);
        }
    }
}
