namespace SmartTechTest.Main.State
{
    using Game.Mobs;
    using Player;
    using Zenject;

    public class GameState : AppState
    {
        public override void Init(DiContainer container)
        {
            AddStage(new PlayerInstanceController(), container);
            AddStage(new MobsController(), container);
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
                stage.Value.Stop(false);
            }
        }
    }
}
