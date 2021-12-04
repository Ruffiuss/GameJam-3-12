using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gamekit2D
{
    public sealed class AstralCopyController : MonoBehaviour
    {
        #region Properties

        public byte CurrentCopiesCount { get; private set; }
        public bool IsShieldCooldown { get; private set; }

        #endregion


        #region Fields

        private AstralCopyPool m_astralCopyPool;
        private PlayerCharacter m_eventPublisher;

        private Dictionary<AstralCopyMode, GameObject> m_currentCopies;
        private bool isShieldActive = false;

        public Color AstralCopyColor = Color.blue;
        public float AstralCopyShieldSpawnDistance = 1.0f;
        public float AstralCopyShieldStrength = 1.0f;
        public float AstralCopyShieldCooldown = 5.0f;
        public float AstralCopySpearSpawnDistance = 3.0f;
        public byte MaxAstralCopiesCount = (byte)1;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_astralCopyPool = new AstralCopyPool();
        }

        private void Start()
        {
            m_currentCopies = new Dictionary<AstralCopyMode, GameObject>() { { AstralCopyMode.Shield, null}, {AstralCopyMode.Spear , null}, {AstralCopyMode.Teleport , null} };
        }

        #endregion


        #region ClassLifeCycles

        ~AstralCopyController()
        {
            if (!m_eventPublisher)
            {
                m_eventPublisher.OnAstralCopyShieldHeld -= PutUpShield;
            }
        }

        #endregion


        #region Methods

        internal void Initialize(PlayerCharacter eventPublisher)
        {
            m_eventPublisher = eventPublisher;
            m_eventPublisher.OnAstralCopyShieldHeld += PutUpShield;

            m_currentCopies[AstralCopyMode.Shield] = m_astralCopyPool.Pop();
            m_currentCopies[AstralCopyMode.Shield].SetActive(false);
            m_currentCopies[AstralCopyMode.Shield].GetComponent<SpriteRenderer>().flipX = m_eventPublisher.spriteRenderer.flipX;
            m_currentCopies[AstralCopyMode.Shield].GetComponent<SpriteRenderer>().color = AstralCopyColor;
            m_currentCopies[AstralCopyMode.Shield].transform.SetParent(transform);
            m_currentCopies[AstralCopyMode.Shield].transform.position = new Vector2(m_eventPublisher.transform.position.x + AstralCopyShieldSpawnDistance * m_eventPublisher.GetFacing(), m_eventPublisher.transform.position.y);
            m_currentCopies[AstralCopyMode.Shield].GetComponent<Animator>().SetTrigger("Defence");
            m_currentCopies[AstralCopyMode.Shield].GetComponent<AstralCopyView>();
        }

        internal void PutUpShield(bool hasInput)
        {
            if (hasInput && !isShieldActive)
            {
                if (CurrentCopiesCount >= MaxAstralCopiesCount)
                    return;

                m_currentCopies[AstralCopyMode.Shield].SetActive(true);
                CurrentCopiesCount++;
                isShieldActive = true;
            }
            else if (!hasInput && isShieldActive)
            {
                m_currentCopies[AstralCopyMode.Shield].SetActive(false);
                isShieldActive = false;
                IsShieldCooldown = true;
                CurrentCopiesCount--;
                StartCoroutine(ShieldCooldown());
            }
        }

        IEnumerator ShieldCooldown()
        {
            yield return new WaitForSeconds(AstralCopyShieldCooldown);
            StopCoroutine("ShieldCooldown");
            IsShieldCooldown = false;
        }

        #endregion
    }
}
