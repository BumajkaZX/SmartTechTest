namespace SmartTechTest.Main.State
{
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class PauseController : MonoBehaviour
    {
        [Inject]
        private IStateSystem _appStateSystem;

        [SerializeField]
        private Transform _rootObject;

        [SerializeField]
        private Button _pauseButton;

        [SerializeField]
        private Button _resumeButton;

        [SerializeField]
        private Button _restartButton;

        private void Awake()
        {
            Subscribe();
            _rootObject.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(false);
        }

        private void Subscribe()
        {
            _appStateSystem.OnStateChange.Subscribe(state =>
            {
                _pauseButton.gameObject.SetActive(state is GameState);    
            }).AddTo(this);
            
            _pauseButton.OnClickAsObservable().Subscribe(_ => OnPause()).AddTo(this);
            _resumeButton.OnClickAsObservable().Subscribe(_ => OnResume()).AddTo(this);
            _restartButton.OnClickAsObservable().Subscribe(_ => OnRestart()).AddTo(this);
        }

        private void OnPause()
        {
            _rootObject.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _appStateSystem.RequestPause(true);
        }

        private void OnResume()
        {
            _rootObject.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _appStateSystem.RequestPause(false);
        }

        private void OnRestart()
        {
            _rootObject.gameObject.SetActive(false);
            _appStateSystem.RequestState(new GameState(), true);
        }
    }
}
