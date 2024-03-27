namespace SmartTechTest.Register
{
    using Game.Field;
    using Game.Mobs;
    using Game.Spawn;
    using Main.Pool;
    using Main.State;
    using UnityEngine;
    using Zenject;

    public class AppInstaller : MonoInstaller
    {
        [Header("Game Instances")]
        
        [SerializeField]
        private BaseField _baseField;

        [SerializeField]
        private SpawnPoints _spawnPoints;
        
        public override void InstallBindings()
        {
            RegisterInput();
            RegisterGameField();
            RegisterAppStateSystem();
            RegisterParticlesPool();
            RegisterMobFactory();
            RegisterSpawnPoints();
        }

        /// <summary>
        /// Регистрация инпут системы
        /// </summary>
        private void RegisterInput()
        {
            PlayerControl control = new PlayerControl();
            control.Enable();

            Container.BindInstance(control).AsSingle();
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
    }
}
