namespace SmartTechTest.UI
{
    using UnityEngine;

    /// <summary>
    /// Нужен для включения\выключения UI
    /// </summary>
    public class PlayerInputView : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
