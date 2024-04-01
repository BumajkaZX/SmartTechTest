namespace SmartTechTest.Game.Mobs
{
    using Field;
    using Main.Mob;
    using Main.Spawn;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Pool;
    using Zenject;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;
    
    public class MobFactory : IMobFactory, IDisposable
    {
        /// <summary>
        /// Извещает о спавне мобов
        /// </summary>
        public ReactiveCommand<List<MobViewController>> OnMobSpawned { get; } 
        
        private const string MOBS_PATH = "Mobs/";

        private const string BASE_PREFAB_MOB = "Mobs/Base/MobBase";

        private const int POOL_CAPACITY = 25;
        
        private const float DISTANCE_BETWEEN_LINES = 1f;

        private const float DISTANCE_BETWEEN_MOBS = 0.2f;

        [Inject]
        private IPrefabSpawner _prefabSpawner;

        [Inject]
        private BaseField _baseField;
        
        private List<AbstractMob> _mobs;

        private MobViewController _mobViewController;

        private ObjectPool<MobViewController> _mobPool;
        
        public MobFactory()
        {
            OnMobSpawned = new ReactiveCommand<List<MobViewController>>();
            
            _mobs = Resources.LoadAll<AbstractMob>(MOBS_PATH).ToList();
            _mobViewController = Resources.Load<MobViewController>(BASE_PREFAB_MOB);

            CreatePool();
        }

        public List<MobViewController> SpawnMob(int count, int lines, params string[] arg)
        {
            float finalDistance = _mobViewController.MobBounds.size.x + DISTANCE_BETWEEN_MOBS;
            
            List<MobViewController> spawnedList = new List<MobViewController>();
            
            for (int i = 0; i < lines; i++)
            {
                float minYPos = _baseField.FieldBounds.max.y - i * DISTANCE_BETWEEN_LINES;
                Vector3 startPos = new Vector3(_baseField.FieldBounds.center.x  -  (count / 2) * finalDistance , minYPos, 0);
                
                for (int j = 0; j < count; j++)
                {
                    var currentMob = _mobs[Random.Range(0, _mobs.Count)];
                    
                    MobViewController spawnedMob = _mobPool.Get();
                    spawnedMob.transform.position = startPos + new Vector3(finalDistance * j, 0, 0);
                    spawnedMob.Init(currentMob.Sprite, currentMob);
                    
                    spawnedList.Add(spawnedMob);
                }
            }

            OnMobSpawned.Execute(spawnedList);
            
            return spawnedList;
        }

        public void ReleaseMob(MobViewController mob)
        {
            _mobPool.Release(mob);
        }

        public void ReleaseMob(List<MobViewController> mobs)
        {
            for (int i = 0; i < _mobs.Count; i++)
            {
                _mobPool.Release(mobs[i]);
            }
        }

        private void CreatePool()
        {
            _mobPool = new ObjectPool<MobViewController>(() => _prefabSpawner.Instantiate(_mobViewController),
                mob => mob.gameObject.SetActive(true),
                mob => mob.gameObject.SetActive(false),
                mob =>
                {
                    if (mob != null)
                    {
                        Object.Destroy(mob.gameObject);
                    }
                },
                defaultCapacity: POOL_CAPACITY);
        }

        public void Dispose()
        {
            _mobPool.Clear();
        }
    }
}
