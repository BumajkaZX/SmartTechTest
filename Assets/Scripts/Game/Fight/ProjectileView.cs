namespace SmartTechTest.Game.Fight
{
    using UnityEngine;

    public class ProjectileView : MonoBehaviour
    {
        public Vector3 MoveDirectory => _moveDir;
        
        public Collider2D Collider => _collider2D;
        
        [SerializeField]
        private Collider2D _collider2D;

        private Vector3 _moveDir;

        public void SetMoveDirectory(Vector3 dir) => _moveDir = dir;
    }
}
