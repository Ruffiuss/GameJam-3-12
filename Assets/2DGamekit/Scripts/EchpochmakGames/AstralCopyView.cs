using System;
using UnityEngine;


namespace Gamekit2D
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    internal sealed class AstralCopyView : MonoBehaviour
    {
        #region Properties

        public event Action<GameObject> OnCollision;

        #endregion


        #region Fields

        public Damageable damageable;
        public Damager meleeDamager;

        private SpriteRenderer m_spriteRenderer;
        private CapsuleCollider2D m_capsuleCollider;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_capsuleCollider = GetComponent<CapsuleCollider2D>();
            damageable = GetComponent<Damageable>();
            meleeDamager = GetComponent<Damager>();
        }

        #endregion


        #region Methods

        public AstralCopyView SetupAsShield(int health)
        {
            damageable.disableOnDeath = false;
            damageable.GainHealth(health);

            return this;
        }

        public void ShieldDamaged()
        {
            OnCollision.Invoke(gameObject);
        }

        public void Unload()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
