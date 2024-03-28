namespace SmartTechTest.Game.Fight
{
    using Main.State;
    using System.Collections;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Pool;

    public class FightSystem : IStateStage
    {
        private const string PROJECTILE_PREFAB_PATH = "Projectile/ProjectileBase";

        private const int POOL_CAPACITY = 20;

        private ProjectileView _projectile;

        private CompositeDisposable _disposable;

        private ObjectPool<ProjectileView> _projectilesPool;

        public void RequestHit(Vector3 pos, Vector3 dir)
        {
            
        }

        public void Init()
        {
            _projectile = Resources.Load<ProjectileView>(PROJECTILE_PREFAB_PATH);
            _disposable = new CompositeDisposable();

            InitPool();
        }

        public void Start()
        {
            
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                _projectilesPool.Clear();
                _disposable.Clear();
                return;
            }
        }

        private void MoveUpdate()
        {
            
        }

        private void SubscribeToUpdate()
        {
            
        }

        private void InitPool()
        {
            _projectilesPool =
                new ObjectPool<ProjectileView>(
                    () => Object.Instantiate(_projectile), 
                    actionOnDestroy: view => Object.Destroy(view.gameObject),
                    defaultCapacity: POOL_CAPACITY);
        }

        public void Pause(bool isPaused)
        {
            if (isPaused)
            {
                _disposable.Clear();
                return;
            }

            SubscribeToUpdate();
        }
    }
}
