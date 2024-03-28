namespace SmartTechTest.Main.Spawn
{
    public interface IPrefabSpawner
    {
        public T Instantiate<T>(T prefabType) where T : UnityEngine.Object;
    }
}
