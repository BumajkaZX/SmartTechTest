namespace SmartTechTest.Main.Player
{
    using Game.Spawn;
    using Spawn;
    using State;
    using UnityEngine;
    using Zenject;

    /// <summary>
    /// Управляет спавном игрока на поле
    /// </summary>
    public class PlayerInstanceController : IStateStage
    {
        private const string PLAYER_PATH = "Player/PlayerBase";

        [Inject]
        private SpawnPoints _spawnPoints;

        [Inject]
        private IPrefabSpawner _prefabSpawner;

        private PlayerController _playerController;

        private void BeginGame()
        {
            _playerController =
                _prefabSpawner.Instantiate(_playerController);
        }

        private void ResetPlayer()
        {
            _playerController.transform.position = _spawnPoints.PlayerSpawnTransform.position;
            _playerController.gameObject.SetActive(true);
        }

        public void Init()
        {
            _playerController = Resources.Load<PlayerController>(PLAYER_PATH);
            BeginGame();
        }

        public void Start()
        {
            ResetPlayer();
        }

        public void Stop(bool shouldClearResources)
        {
            if (shouldClearResources)
            {
                if (_playerController != null)
                {
                    Object.Destroy(_playerController.gameObject);
                }
                return;
            }
            
            _playerController.gameObject.SetActive(false);
        }

        public void Pause(bool isPaused)
        {
            
        }
    }
}
