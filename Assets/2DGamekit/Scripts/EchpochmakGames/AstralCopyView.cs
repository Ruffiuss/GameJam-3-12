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
        public Damager damager;

        private SpriteRenderer m_spriteRenderer;
        private CapsuleCollider2D m_capsuleCollider;
        private bool isDamaged = false;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_capsuleCollider = GetComponent<CapsuleCollider2D>();
            damageable = GetComponent<Damageable>();
            damager = GetComponent<Damager>();
        }

        #endregion


        #region Methods

        public AstralCopyView SetupAsShield(int health)
        {
            damageable.disableOnDeath = false;
            damageable.GainHealth(health);
            isDamaged = false;

            return this;
        }

        public void ShieldDamaged()
        {
            if (!isDamaged)
            {
                isDamaged = true;
                OnCollision.Invoke(gameObject);
            }
        }

        public void Unload()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}
