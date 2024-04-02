namespace SmartTechTest.Register
{
    using Game.Field;
    using Game.Mobs;
    using Game.Spawn;
    using Main.Pool;
    using Main.Spawn;
    using Main.State;
    using UI;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        [Header("Game Instances")]
        
        [SerializeField]
        private BaseField _baseField;

        [SerializeField]
        private SpawnPoints _spawnPoints;

        [SerializeField]
        private GameCounterView _gameCounterView;

        [SerializeField]
        private PlayerInputView _playerInputView;
        
        public override void InstallBindings()
        {
            RegisterInput();
            RegisterGameField();
            RegisterAppStateSystem();
            RegisterParticlesPool();
            RegisterMobFactory();
            RegisterSpawnPoints();
            RegisterSpawner();
            RegisterProjectilesPool();
            RegisterGameCounter();
        }

        /// <summary>
        /// Регистрация инпут системы
        /// </summary>
        private void RegisterInput()
        {
            PlayerControl control = new PlayerControl();
            control.Enable();

            Container.BindInstance(control).AsSingle();

            Container.BindInstance(_playerInputView).AsSingle();
        }

        /// <summary>
        /// Регистрация игрового поля
        /// </summary>
        private void RegisterGameField()
        {
            Container.BindInstance(_baseField).AsSingle();
        }

        /// <summary>
        /// Регистрация системы состояний
        /// </summary>
        private void RegisterAppStateSystem()
        {
            Container.BindInterfacesAndSelfTo<AppStateSystem>().AsSingle();
        }

        /// <summary>
        /// Регистрация пула частиц
        /// </summary>
        private void RegisterParticlesPool()
        {
            Container.BindInterfacesAndSelfTo<ParticlesPool>().AsTransient();
        }

        /// <summary>
        /// Регистрация фабрики мобов
        /// </summary>
        private void RegisterMobFactory()
        {
            Container.BindInterfacesAndSelfTo<MobFactory>().AsSingle();
        }

        /// <summary>
        /// Регистрация точек спавна игрока\мобов
        /// </summary>
        private void RegisterSpawnPoints()
        {
            Container.BindInstance(_spawnPoints).AsSingle();
        }

        /// <summary>
        /// Регистрация расширения для спавна обьектов с инджекцией
        /// </summary>
        private void RegisterSpawner()
        {
            Container.BindInterfacesAndSelfTo<ContainerPrefabSpawner>().AsSingle();
        }

        /// <summary>
        /// Регистрация пула проджектайлов
        /// </summary>
        private void RegisterProjectilesPool()
        {
            Container.BindInterfacesAndSelfTo<ProjectilesPool>().AsTransient();
        }

        /// <summary>
        /// Регистрация счётчика очков
        /// </summary>
        private void RegisterGameCounter()
        {
            Container.Bind<IGameCounter>().FromInstance(_gameCounterView).AsSingle();
        }
    }
}
