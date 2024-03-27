namespace SmartTechTest.Game.Field
{
    using UnityEngine;

    /// <summary>
    /// Ограничитель игрового поля
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class BaseField : MonoBehaviour
    {
        public Bounds FieldBounds => _boxCollider2D.bounds;
        
        [SerializeField]
        protected BoxCollider2D _boxCollider2D;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (_boxCollider2D == null)
            {
                _boxCollider2D = GetComponent<BoxCollider2D>();
            }
        }

#endif
        
    }
}
