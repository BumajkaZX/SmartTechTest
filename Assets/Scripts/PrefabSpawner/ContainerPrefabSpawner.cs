namespace SmartTechTest.Main.Spawn
{
    using System;
    using Zenject;

    public class ContainerPrefabSpawner : IPrefabSpawner
    {
        [Inject]
        private DiContainer _container;

        public T Instantiate<T>(T prefabType) where T:  UnityEngine.Object
        {
           var spawnedObject = UnityEngine.Object.Instantiate(prefabType);
           
           _container.Inject(spawnedObject);

           return spawnedObject;
        }
    }
}
