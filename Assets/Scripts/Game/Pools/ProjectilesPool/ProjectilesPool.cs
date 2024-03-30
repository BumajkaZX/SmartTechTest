namespace SmartTechTest.Main.Pool
{
    using Game.Fight;
    using Spawn;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;
    using Zenject;
    using Object = UnityEngine.Object;

    public class ProjectilesPool : IGamePool<ProjectileView>
    {
        private Dictionary<ProjectileView, ObjectPool<ProjectileView>> _poolDictionary;

        public ProjectilesPool()
        {
            _poolDictionary = new Dictionary<ProjectileView, ObjectPool<ProjectileView>>();
        }

        public Action<ProjectileView> RequestObject(ProjectileView baseObject, out ProjectileView returnedObjectFromPool)
        {
            if (!_poolDictionary.ContainsKey(baseObject))
            {
                CreatePool(baseObject);
            }

            returnedObjectFromPool = _poolDictionary[baseObject].Get();
            return Release;
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
                    actionOnDestroy: view => Object.Destroy(view.gameObject),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(baseObject, newPool);
        }

        private void Release(ProjectileView projectile)
        {
            if (!_poolDictionary.TryGetValue(projectile, out var pool))
            {
                Object.Destroy(projectile.gameObject);
                return;
            }

            pool.Release(projectile);
        }
    }
}
