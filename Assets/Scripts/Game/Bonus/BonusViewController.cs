namespace SmartTechTest.Game.Bonus
{
    using Fight;
    using UnityEngine;

    public class BonusViewController : MonoBehaviour
    {
        public BaseGun GunBonus => _gunBonus;

        public Collider2D Collider2D => _collider;

        public float BonusTime => _bonusTime;

        public float DropSpeed => _dropSpeed;

        [SerializeField]
        private BaseGun _gunBonus;

        [SerializeField]
        private Collider2D _collider;

        [SerializeField]
        private SpriteRenderer _icon;

        [SerializeField]
        private float _bonusTime;
        
        private float _dropSpeed;

        public void SetBonus(Bonus bonus)
        {
            _gunBonus = bonus.Gun;
            _bonusTime = bonus.BonusTime;
            _icon.sprite = bonus.BonusIcon;
            _dropSpeed = bonus.BonusDropSpeed;
        }
    }
}
