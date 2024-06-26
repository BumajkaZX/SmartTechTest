namespace SmartTechTest.Game.Fight
{
    using UnityEngine;

    public class ProjectileView : MonoBehaviour
    {
        public Vector2 MoveDirectory => _moveDir;
        
        public Collider2D Collider => _collider2D;

        public LayerMask Target => _target;

        [SerializeField]
        private Collider2D _collider2D;

        private Vector2 _moveDir;

        private BaseGun _associatedGun;

        private LayerMask _target;

        public void SetMoveDirectory(Vector2 dir) => _moveDir = dir;

        public void SetTarget(LayerMask target) => _target = target;
    }
}
