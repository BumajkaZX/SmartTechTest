namespace SmartTechTest.Main.State
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public abstract class AppState
    {
        protected Dictionary<int, IStateStage> _stateStages = new Dictionary<int, IStateStage>();

        protected List<IPauseStage> _pauseStages = new List<IPauseStage>();
        
        public abstract void Init(DiContainer container);
        
        public abstract void Enter();

        public abstract void Exit(bool shouldClearResources);

        public virtual void RequestPause(bool isPaused)
        {
            foreach (var pauseStage in _pauseStages)
            {
                pauseStage.Pause(isPaused);
            }

            foreach (var stageByKey in _stateStages)
            {
                stageByKey.Value.Pause(isPaused);
            }
        }

        protected void AddPauseStage(IPauseStage stage, DiContainer container)
        {
            if (_pauseStages.Contains(stage))
            {
                Debug.LogWarning($"{stage.GetType().FullName} is already in pause stages");
                return;
            }
            
            _pauseStages.Add(stage);
            container.Inject(stage);
            stage.Init();
        }
        
        protected void AddStage(IStateStage stage, DiContainer container)
        {
            if (!_stateStages.TryAdd(stage.GetHashCode(), stage))
            {
                Debug.LogWarning($"{stage.GetType().FullName} is already in stages");
                return;
            }
            
            container.Inject(stage);
            stage.Init();
        }
    }
}
