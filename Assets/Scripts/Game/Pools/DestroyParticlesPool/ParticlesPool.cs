namespace SmartTechTest.Main.Pool
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;
    
    /// <summary>
    /// Пул для частиц
    /// </summary>
    public class ParticlesPool : IGamePool<ParticleSystem>
    {
        private Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> _poolDictionary;

        private ParticlesPool()
        {
            _poolDictionary = new Dictionary<ParticleSystem, ObjectPool<ParticleSystem>>();
        }

        private void CreatePool(ParticleSystem baseObject, int estimatedCapacity = 10)
        {
            if (_poolDictionary.ContainsKey(baseObject))
            {
                return;
            }

            ObjectPool<ParticleSystem> newPool =
                new ObjectPool<ParticleSystem>(
                    () => Object.Instantiate(baseObject),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(baseObject, newPool);
        }

        public Action<ParticleSystem> RequestObject(ParticleSystem baseObject, out ParticleSystem returnedObjectFromPool)
        {
            if (!_poolDictionary.ContainsKey(baseObject))
            {
                CreatePool(baseObject);
            }

            returnedObjectFromPool = _poolDictionary[baseObject].Get();
            return Release;
        }

        private void Release(ParticleSystem particleSystem)
        {
            if (!_poolDictionary.TryGetValue(particleSystem, out var pool))
            {
                Object.Destroy(particleSystem.gameObject);
                return;
            }
            
            pool.Release(particleSystem);
        }
    }
}
