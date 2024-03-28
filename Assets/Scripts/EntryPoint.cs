namespace SmartTechTest.Entry
{
    using Main.State;
    using UnityEngine;
    using Zenject;

    public class EntryPoint : MonoBehaviour
    {
        [Inject]
        private AppStateSystem _appStateSystem;
        
        private void Awake()
        {
            _appStateSystem.RequestState(new MenuState());
        }
    }
}
