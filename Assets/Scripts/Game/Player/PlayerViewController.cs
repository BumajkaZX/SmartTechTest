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

        public float HeightMoveTime => _heightMoveTime;

        public int MaxHeightSteps => _maxHeightSteps;
        
        public BaseGun BaseGun => _currentGun;

        [SerializeField]
        private PlayerView _playerView;

        [Header("Player settings")]

        [SerializeField, Min(0.01f)]
        private float _moveSpeed;

        [SerializeField]
        private BaseGun _currentGun;

        [SerializeField]
        private int _maxHeightSteps;

        [SerializeField]
        private float _heightMoveTime;

    }
}
