namespace SmartTechTest.Game.Mobs
{
    using Main.Mob;
    using UnityEngine;

    /// <summary>
    /// Вьюшка моба - небольшой кэш позиции
    /// </summary>
    public class MobViewController : MonoBehaviour
    {
        public AbstractMob MobConfig { get; private set; }

        public Bounds MobBounds => _spriteRenderer.bounds;
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private float _defaultXPos;

        public void Init(Sprite sprite, AbstractMob configMob)
        {
            _spriteRenderer.sprite = sprite;
            MobConfig = configMob;
            _defaultXPos = transform.position.x;
        }

        public void Move(float offsetX)
        {
            transform.position = new Vector3(_defaultXPos + offsetX,transform.position.y, transform.position.z);
        }
    }
}
