namespace SmartTechTest.Game.Fight
{
    using Main.Pool;
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Система спавна проджектайлов
    /// </summary>
    public class ProjectileRequestSystem : IProjectileRequest, IDisposable
    {
        public ReactiveCommand<ProjectileView> OnViewSpawned { get; } = new ReactiveCommand<ProjectileView>();
        public ReactiveCommand<ProjectileView> OnViewReleased { get; } = new ReactiveCommand<ProjectileView>();
        
        [Inject]
        private IGamePool<ProjectileView> _projectilesPool;
        
        private List<ProjectileView> _spawnedViews = new List<ProjectileView>();
        
        private CompositeDisposable _disposable = new CompositeDisposable();
        
        public void RequestProjectile(BaseGun gun, Vector3 pos, Vector3 dir, LayerMask target)
        {
            var releaseAction = _projectilesPool.RequestObject(gun.ProjectileView, out var spawnedProjectile);

            spawnedProjectile.gameObject.ObserveEveryValueChanged(go => go.activeSelf).Where(active => !active)
                .Subscribe(_ =>
                {
                    OnViewReleased.Execute(spawnedProjectile);
                    releaseAction.Invoke(spawnedProjectile);
                }).AddTo(_disposable);
            
            _spawnedViews.Add(spawnedProjectile);

            spawnedProjectile.transform.position = pos;
            
            spawnedProjectile.SetMoveDirectory(dir);
            
            spawnedProjectile.SetTarget(target);
            
            OnViewSpawned.Execute(spawnedProjectile);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
