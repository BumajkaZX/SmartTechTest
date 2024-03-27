namespace SmartTechTest.Main.State
{
    public abstract class AppState
    {
        public virtual void Enter(out AppState possibleTransition)
        {
            possibleTransition = null;
        }
    }
}
