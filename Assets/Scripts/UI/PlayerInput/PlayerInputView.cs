namespace SmartTechTest.UI
{
    using UnityEngine;

    /// <summary>
    /// Нужен для включения\выключения UI
    /// </summary>
    public class PlayerInputView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        private float _defaultAlpha;
        
        private void Awake()
        {
            _defaultAlpha = _canvasGroup.alpha;
            Enable(false);
        }

        public void Enable(bool isEnabled)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            _canvasGroup.alpha = isEnabled ? _defaultAlpha : 0;
            _canvasGroup.blocksRaycasts = isEnabled;
        }
    }
}
