namespace SmartTechTest.Main.State
{
    using System.Collections.Generic;
    using UnityEngine;
    using Zenject;

    public abstract class AppState
    {
        protected Dictionary<int, IStateStage> _stateStages = new Dictionary<int, IStateStage>();

        protected bool _isInited;
        
        public abstract void Init(DiContainer container);
        
        public abstract void Enter();

        public abstract void Exit(bool shouldClearResources);

        public void Pause(bool isPaused)
        {
            foreach (var state in _stateStages)
            {
                state.Value.Pause(isPaused);
            }
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
