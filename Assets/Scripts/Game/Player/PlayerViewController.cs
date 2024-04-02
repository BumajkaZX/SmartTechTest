namespace SmartTechTest.Main.Player
{
    using Game.Fight;
    using UnityEngine;
    
    /// <summary>
    /// Контроллер для настроек и управления
    /// </summary>
    public class PlayerViewController : MonoBehaviour
    {

        public PlayerView PlayerView => _playerView;

        public float MoveSpeed => _moveSpeed;

        public BaseGun BaseGun => _currentGun;

        [SerializeField]
        private PlayerView _playerView;

        [Header("Player settings")]

        [SerializeField, Min(0.01f)]
        private float _moveSpeed;

        [SerializeField]
        private BaseGun _currentGun;
        
    }
}
