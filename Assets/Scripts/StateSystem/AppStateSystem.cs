namespace SmartTechTest.Main.State
{
    using System;
    using UniRx;

    public class AppStateSystem : IStateSystem
    {
        public ReactiveCommand<AppState> OnStateChange { get; } = new ReactiveCommand<AppState>();
        
        public void ChangeState(AppState newState)
        {
            OnStateChange.Publish(newState);
            newState.Enter(out var possibleTransition);
            if (possibleTransition == null)
            {
                return;
            }
            OnStateChange.Publish(possibleTransition);
        }
    }
}