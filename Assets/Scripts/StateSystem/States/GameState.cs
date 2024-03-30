namespace SmartTechTest.Main.State
{
    using Game.Fight;
    using Game.Mobs;
    using Player;
    using Zenject;

    public class GameState : AppState
    {
        private DiContainer _container;
        
        public override void Init(DiContainer container)
        {
            _container = container;
            
            container.BindInterfacesAndSelfTo<ProjectileRequestSystem>().AsSingle();
            container.BindInterfacesAndSelfTo<HitObserver>().AsSingle();
            
            AddStage(new PlayerInstanceController(), container);
            AddStage(new MobsController(), container);
            AddStage(new HitDetectionSystem(), container);
            
            AddPauseStage(new ProjectileMovementSystem(), container);
        }

        public override void Enter()
        {
            foreach (var stage in _stateStages)
            {
                stage.Value.Start();
            }
        }

        public override void Exit(bool shouldClearResources)
        {
            foreach (var stage in _stateStages)
            {
                stage.Value.Stop(shouldClearResources);
            }

            _container.Unbind<ProjectileRequestSystem>();
        }
    }
}
