namespace SmartTechTest.Game.Mobs
{
    using Field;
    using Main.Mob;
    using Main.Pool;
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Контроллер для управления перемещением мобов и их атаки
    /// </summary>
    public class MobsController : IDisposable
    {
        [Inject]
        private IGamePool<ParticleSystem> _destroyPool;

        [Inject]
        private IMobFactory _mobFactory;

        [Inject]
        private BaseField _baseField;

        private List<MobViewController> _viewControllers;

        private CompositeDisposable _disposable;

        public MobsController()
        {
            _disposable = new CompositeDisposable();
            _viewControllers = new List<MobViewController>();
            Observable.EveryUpdate().Subscribe(_ => Update());
        }

        //TODO: Реализовать паттерны спавна
        public void SpawnWave(int mobCount)
        {
            for (int i = 0; i < mobCount; i++)
            {
                //_viewControllers.Add(_mobFactory.SpawnMob());
            }
        }

        private void Update()
        {
            for (int i = 0; i < _viewControllers.Count; i++)
            {
                _viewControllers[i].Move(Vector2.left);
            }
        }

        public void Dispose()
        {
            _disposable.Clear();
        }
    }
}
