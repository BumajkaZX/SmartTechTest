namespace SmartTechTest.Game.Spawn
{
    using UnityEngine;

    /// <summary>
    /// Точки спавна
    /// </summary>
    public class SpawnPoints : MonoBehaviour
    {
        public Transform PlayerSpawnTransform => _playerSpawnTransform;

        public Transform MobSpawnTransform => _mobSpawnTransform;

        [SerializeField]
        private Transform _playerSpawnTransform;

        [SerializeField]
        private Transform _mobSpawnTransform;
    }
}