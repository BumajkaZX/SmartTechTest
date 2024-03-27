namespace SmartTechTest.Main.State
{
    using System;
    using UniRx;

    public interface IStateSystem
    {
        public ReactiveCommand<AppState> OnStateChange { get; }

        public void ChangeState(AppState newState);
    }
}
