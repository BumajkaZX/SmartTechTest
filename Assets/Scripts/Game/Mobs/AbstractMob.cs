namespace SmartTechTest.Main.Mob
{
    using Pool;
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
        
        [SerializeField]
        protected Sprite _sprite;

        [SerializeField]
        protected ParticleSystem _destroyParticles;

        /// <summary>
        /// Отрабатывает при попадании в моба
        /// </summary>
        /// <param name="currentPos">Текущая позиция моба</param>
        /// <param name="particlesPool">Партиклы уничтожения -отрабатывает в случае смерти моба</param>
        /// <returns></returns>
        public abstract bool HitDetect(Transform currentPos, IGamePool<ParticleSystem> particlesPool);
    }
}
