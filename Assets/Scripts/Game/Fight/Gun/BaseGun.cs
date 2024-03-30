namespace SmartTechTest.Game.Fight
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Game/Gun", fileName = "Gun")]
    public class BaseGun : ScriptableObject
    {
        public float ShootSpeed => _shootSpeed;

        public float ProjectileSpeed => _projectileSpeed;

        public ProjectileView ProjectileView => _projectileView;

        [SerializeField]
        [Header("Скорость стрельбы")]
        protected float _shootSpeed;

        [SerializeField]
        [Header("Скорость перемещения проджектайла")]
        protected float _projectileSpeed;
        
        [SerializeField]
        protected ProjectileView _projectileView;
    }
}
