namespace SmartTechTest.Main.Pool
{
    using Game.Fight;
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;

    public class ProjectilesPool : IGamePool<ProjectileView>
    {
        private Dictionary<ProjectileView, ObjectPool<ProjectileView>> _poolDictionary;

        private CompositeDisposable _disposable;
        
        public ProjectilesPool()
        {
            _poolDictionary = new Dictionary<ProjectileView, ObjectPool<ProjectileView>>();
            _disposable = new CompositeDisposable();
        }

        public ReactiveCommand RequestObject(ProjectileView baseObject, out ProjectileView returnedObjectFromPool)
        {
            if (!_poolDictionary.ContainsKey(baseObject))
            {
                CreatePool(baseObject);
            }

            ReactiveCommand releaseCommand = new ReactiveCommand();

            var projectile = _poolDictionary[baseObject].Get();

            releaseCommand.Subscribe(_ =>
            {
                Release(baseObject, projectile);
                releaseCommand.Dispose();
            }).AddTo(_disposable);

            returnedObjectFromPool = projectile;

            return releaseCommand;
        }

        private void CreatePool(ProjectileView baseObject, int estimatedCapacity = 10)
        {
            if (_poolDictionary.ContainsKey(baseObject))
            {
                return;
            }

            ObjectPool<ProjectileView> newPool =
                new ObjectPool<ProjectileView>(
                    () => Object.Instantiate(baseObject),
                    view => view.gameObject.SetActive(true),
                    actionOnDestroy: view => Object.Destroy(view.gameObject),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(baseObject, newPool);
        }

        private void Release(ProjectileView baseProjectile, ProjectileView projectile)
        {
            if (!_poolDictionary.TryGetValue(baseProjectile, out var pool))
            {
                Object.Destroy(projectile.gameObject);
                return;
            }

            pool.Release(projectile);
        }

        public void Dispose()
        {
            _disposable?.Dispose();

            foreach (var keyValue in _poolDictionary)
            {
                keyValue.Value?.Dispose();
            }
        }
    }
}
