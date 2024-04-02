namespace SmartTechTest.UI
{
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameCounterView : MonoBehaviour, IGameCounter
    {
        [SerializeField]
        private TextMeshProUGUI _counterText;

        private int _current;

        private void Awake()
        {
            gameObject.SetActive(false);
            _counterText.text = "0";
        }

        public void Increase(int count)
        {
            _current += count;
            _counterText.text = _current.ToString();
        }

        public void Clear()
        {
            _current = 0;
            _counterText.text = "0";
        }

        public void Enable(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_counterText == null)
            {
                _counterText = GetComponent<TextMeshProUGUI>();
            }
        }

#endif
        
    }
}
