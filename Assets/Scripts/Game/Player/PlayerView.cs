namespace SmartTechTest.Main.Player
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerView : MonoBehaviour
    {
        public Bounds SpriteBounds => _spriteRenderer.bounds;
        
        [SerializeField]
        private Sprite _defaultSprite;

        [SerializeField]
        private Sprite _leftSide;

        [SerializeField]
        private Sprite _rightSide;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer.sprite = _defaultSprite;
        }

        public void UpdateView(Vector2 dir)
        {
            switch (dir.x)
            {
                case > 0:
                    _spriteRenderer.sprite = _rightSide;
                    break;

                case < 0:
                    _spriteRenderer.sprite = _leftSide;
                    break;

                default:
                    _spriteRenderer.sprite = _defaultSprite;
                    break;
            }
        }
    }
}
