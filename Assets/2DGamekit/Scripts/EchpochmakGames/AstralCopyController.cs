using System.Collections.Generic;
using UnityEngine;


namespace Gamekit2D
{
    public sealed class AstralCopyController : MonoBehaviour
    {
        #region Properties

        private AstralCopyPool m_astralCopyPool;
        private PlayerCharacter m_eventPublisher;

        internal byte CurrentCopiesCount { get; private set; }

        #endregion


        #region Fields

        public Color AstralCopyColor = Color.blue;
        public float AstralCopyShieldSpawnDistance = 1.0f;
        public float AstralCopyShieldStrength = 1.0f;
        public float AstralCopyShieldCooldown = 5.0f;
        public float AstralCopySpearSpawnDistance = 3.0f;
        public byte MaxAstralCopiesCount = (byte)1; // add to editor

        private Dictionary<AstralCopyMode, GameObject> m_currentCopies;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_astralCopyPool = new AstralCopyPool();
        }

        private void Start()
        {
            m_currentCopies = new Dictionary<AstralCopyMode, GameObject>();
        }

        #endregion


        #region ClassLifeCycles

        ~AstralCopyController()
        {
            if (!m_eventPublisher)
            {
                m_eventPublisher.OnAstralCopyUsed -= MakeAstralCopy;
            }
        }

        #endregion


        #region Methods

        internal void Initialize(PlayerCharacter eventPublisher)
        {
            m_eventPublisher = eventPublisher;
            m_eventPublisher.OnAstralCopyUsed += MakeAstralCopy;
        }

        internal void MakeAstralCopy(AstralCopyMode mode)
        {
            if (CurrentCopiesCount < MaxAstralCopiesCount)
            {
                if (m_currentCopies.ContainsKey(mode))
                {
                    if (!m_currentCopies[mode])
                    {
                        m_currentCopies[mode] = m_astralCopyPool.Pop();
                        SetAstralCopyComponents(m_currentCopies[mode]);
                        CurrentCopiesCount++;
                    }
                    else Debug.Log($"{mode} alredy have view on scene: {m_currentCopies[mode].name}");
                }
                else
                {
                    m_currentCopies.Add(mode, m_astralCopyPool.Pop());
                    SetAstralCopyComponents(m_currentCopies[mode]);
                    CurrentCopiesCount++;
                }
            }
            Debug.Log($"You already use max count of copies");
        }

        private void SetAstralCopyComponents(GameObject original)
        {
            original.transform.position = new Vector2(m_eventPublisher.transform.position.x + AstralCopyShieldSpawnDistance * m_eventPublisher.GetFacing(), m_eventPublisher.transform.position.y);
            original.GetComponent<CapsuleCollider2D>().offset = m_eventPublisher.CapsuleColliderOffset;
            original.GetComponent<CapsuleCollider2D>().size = m_eventPublisher.CapsuleColliderSize;
            original.GetComponent<SpriteRenderer>().sprite = m_eventPublisher.spriteRenderer.sprite;
            original.GetComponent<SpriteRenderer>().flipX = m_eventPublisher.spriteRenderer.flipX;
            original.GetComponent<SpriteRenderer>().color = AstralCopyColor;
        }

        #endregion
    }
}
