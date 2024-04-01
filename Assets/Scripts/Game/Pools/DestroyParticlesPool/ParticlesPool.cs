namespace SmartTechTest.Main.Pool
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Pool;
    using Object = UnityEngine.Object;
    
    /// <summary>
    /// Пул для частиц
    /// </summary>
    public class ParticlesPool : IGamePool<ParticleSystem>
    {
        private Dictionary<ParticleSystem, ObjectPool<ParticleSystem>> _poolDictionary;

        private CompositeDisposable _disposable;

        private ParticlesPool()
        {
            _poolDictionary = new Dictionary<ParticleSystem, ObjectPool<ParticleSystem>>();
            _disposable = new CompositeDisposable();
        }

        public ReactiveCommand RequestObject(ParticleSystem baseObject, out ParticleSystem returnedObjectFromPool)
        {
            if (!_poolDictionary.ContainsKey(baseObject))
            {
                CreatePool(baseObject);
            }

            ReactiveCommand releaseCallback = new ReactiveCommand();
            
            var particlesSystem = _poolDictionary[baseObject].Get();

            releaseCallback.Subscribe(_ =>
            {
                Release(baseObject, particlesSystem);
                releaseCallback.Dispose();
            }).AddTo(_disposable);

            returnedObjectFromPool = particlesSystem;
            
            return releaseCallback;
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
                    system => system.gameObject.SetActive(true),
                    system => system.gameObject.SetActive(false),
                    system => Object.Destroy(system.gameObject),
                    defaultCapacity: estimatedCapacity);

            _poolDictionary.Add(baseObject, newPool);
        }

        private void Release(ParticleSystem baseObject, ParticleSystem particleSystem)
        {
            if (!_poolDictionary.TryGetValue(baseObject, out var pool))
            {
                Object.Destroy(particleSystem.gameObject);
                return;
            }
            
            pool.Release(particleSystem);
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
