namespace SmartTechTest.Main.Mob
{
    using UnityEngine;
    
    /// <summary>
    /// Абстрация моба
    /// </summary>
    public abstract class AbstractMob : ScriptableObject
    {
        /// <summary>
        /// Внешний вид моба
        /// </summary>
        public Sprite Sprite => _sprite;

        public ParticleSystem DestroyParticles => _destroyParticles;
        
        [SerializeField]
        protected Sprite _sprite;

        [SerializeField]
        protected ParticleSystem _destroyParticles;
    }
}
