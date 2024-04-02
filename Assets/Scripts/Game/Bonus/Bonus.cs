namespace SmartTechTest.Game.Bonus
{
    using Fight;
    using UnityEngine;

    /// <summary>
    /// Информация бонуса
    /// </summary>
    [CreateAssetMenu(menuName = "Game/Bonus", fileName = "Bonus")]
    public class Bonus : ScriptableObject
    {
        public float BonusTime => _bonusTime;

        public float BonusDropSpeed => _bonusDropSpeed;

        public Sprite BonusIcon => _bonusIcon;

        public BaseGun Gun => _gun;

        [SerializeField]
        private float _bonusTime;
        
        [SerializeField]
        private float _bonusDropSpeed;
        
        [SerializeField]
        private Sprite _bonusIcon;

        [SerializeField]
        private BaseGun _gun;
    }
}
