namespace SmartTechTest.Main.Player
{
    using Game.Spawn;
    using State;
    using System;
    using UniRx;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Управляет спавном игрока на поле
    /// </summary>
    public class PlayerInstanceController : IDisposable
    {
        private const string PLAYER_PATH = "Player/";

        [Inject]
        private SpawnPoints _spawnPoints;

        [Inject]
        private AppStateSystem _stateSystem;

        private PlayerController _playerController;

        private CompositeDisposable _disposable;

        public PlayerInstanceController()
        {
            _playerController = Resources.Load<PlayerController>(PLAYER_PATH);
            _disposable = new CompositeDisposable();

            SubscribeToState();
        }

        private void SubscribeToState()
        {
            _stateSystem.OnStateChange.Subscribe(state =>
            {
                if (state is GameState)
                {
                    BeginGame();
                    return;
                }

                if (state is RestartState)
                {
                    ResetPlayer();
                    return;
                }

                Dispose();
            }).AddTo(_disposable);
        }

        private void BeginGame()
        {
            _playerController =
                Object.Instantiate(_playerController, _spawnPoints.PlayerSpawnTransform.position, Quaternion.identity);
        }

        private void ResetPlayer()
        {
            _playerController.transform.position = _spawnPoints.PlayerSpawnTransform.position;
        }

        public void Dispose()
        {
            if (_playerController != null)
            {
                Object.Destroy(_playerController.gameObject);
            }

            _disposable.Clear();
        }
    }
}
