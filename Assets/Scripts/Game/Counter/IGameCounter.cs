namespace SmartTechTest.UI
{
    public interface IGameCounter
    {
        public void Increase(int count);

        public void Clear();

        public void Enable(bool isEnabled);
    }
}
