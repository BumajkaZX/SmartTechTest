namespace SmartTechTest.Main.Pool
{
    using System;
    using System.Collections;
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

        public void CreatePool(ParticleSystem particleSystem, int estimatedCapacity = 10)
        {
            if (_poolDictionary.ContainsKey(particleSystem))
            {
                return;
            }

            ObjectPool<ParticleSystem> newPool =
                new ObjectPool<ParticleSystem>(
                    () => Object.Instantiate(particleSystem),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(particleSystem, newPool);
        }

        public Action<ParticleSystem> RequestObject(ParticleSystem baseObject, out ParticleSystem returnedObjectFromPool)
        {
            if (!_poolDictionary.TryGetValue(baseObject, out var pool))
            {
                returnedObjectFromPool = null;
                return Release;
            }

            returnedObjectFromPool = pool.Get();
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
