namespace SmartTechTest.Main.State
{
    public class RestartState : AppState
    {
        public override void Enter(out AppState possibleTransition)
        {
            possibleTransition = new GameState();
        }
    }
}
