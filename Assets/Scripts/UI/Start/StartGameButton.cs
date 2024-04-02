namespace SmartTechTest.UI
{
    using Main.State;
    using System;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        [Inject]
        private IStateSystem _stateSystem;
        
        [SerializeField]
        private Button _button;

        private void Awake()
        {
            _button.OnClickAsObservable().Subscribe(_ =>
            {
                _stateSystem.RequestState(new GameState());
                
                gameObject.SetActive(false);
            });

            _stateSystem.OnStateChange.Subscribe(state =>
            {
               gameObject.SetActive(state is MenuState);
            });
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_button == null)
            {
                _button = GetComponent<Button>();
            }
        }
#endif
    }
}
