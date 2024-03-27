namespace SmartTechTest.Game.Mobs
{
    using Main.Mob;
    using UnityEngine;

    public class MobViewController : MonoBehaviour
    {
        public AbstractMob MobConfig { get; private set; }

        public Bounds MobBounds => _spriteRenderer.bounds;
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        public void Init(Sprite sprite, AbstractMob configMob)
        {
            _spriteRenderer.sprite = sprite;
            MobConfig = configMob;
        }

        public void Move(Vector3 dirPos)
        {
            transform.position += dirPos;
        }
    }
}
