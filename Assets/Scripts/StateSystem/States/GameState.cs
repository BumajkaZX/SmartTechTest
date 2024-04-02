namespace SmartTechTest.Main.State
{
    using Game;
    using Game.Bonus;
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
            
            _container.BindInterfacesAndSelfTo<ProjectileRequestSystem>().AsCached();
            _container.BindInterfacesTo<HitObserver>().AsCached();
            
            AddStage(new PlayerControllerSystem(), container);
            AddStage(new MobsController(), container);
            AddStage(new HitDetectionSystem(), container);
            AddStage(new GameController(), container);
            AddStage(new BonusSystem(), container);
            
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
            
            _container.Resolve<ProjectileRequestSystem>().Dispose();
            _container.Unbind<ProjectileRequestSystem>();
            _container.UnbindInterfacesTo<ProjectileRequestSystem>();
            _container.UnbindInterfacesTo<HitObserver>();
        }
    }
}
