using UnityEngine;


namespace Gamekit2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal sealed class AstralCopyView : MonoBehaviour
    {
        #region Properties

        internal AstralCopyState CurrentState { get; private set; }

        #endregion


        #region Fields

        private SpriteRenderer m_spriteRenderer;
        private CapsuleCollider2D m_capsuleCollider;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_capsuleCollider = GetComponent<CapsuleCollider2D>();
        }

        #endregion
    }
}
