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

        public Color AstralCopyColor = Color.blue;
        public float AstralCopyShieldSpawnDistance = 1.0f;
        public int AstralCopyShieldStrength = 1;
        public float AstralCopyShieldCooldown = 5.0f;
        public float AstralCopySpearSpawnDistance = 3.0f;
        public byte MaxAstralCopiesCount = (byte)1;

        private AstralCopyPool m_astralCopyPool;
        private PlayerCharacter m_eventPublisher;
        private Dictionary<AstralCopyMode, GameObject> m_currentCopies;

        private bool isShieldActive = false;
        private bool isTeleportActive = false;
        private bool isMeleeEnabledBeforeShield;
        private bool isRangedEnabledBeforeShield;
        private bool isTeleportEnabledBeforeShield;

        #endregion


        #region UnityMethods

        private void Start()
        {
            m_currentCopies = new Dictionary<AstralCopyMode, GameObject>() { { AstralCopyMode.Shield, null}, {AstralCopyMode.Spear , null}, {AstralCopyMode.Teleport , null} };
            m_astralCopyPool = new AstralCopyPool();
        }

        #endregion


        #region ClassLifeCycles

        ~AstralCopyController()
        {
            if (!m_eventPublisher)
            {
                m_eventPublisher.OnAstralCopyShieldHeld -= PutUpShield;
                m_eventPublisher.OnAstralCopyTeleportDown -= UseTeleport;
            }
            foreach (var copy in m_currentCopies)
            {
                copy.Value.GetComponent<AstralCopyView>().OnCollision -= ShieldTakeDamage;
                m_astralCopyPool.Push(copy.Value);
            }
        }

        #endregion


        #region Methods

        internal void Initialize(PlayerCharacter eventPublisher)
        {
            m_eventPublisher = eventPublisher;
            m_eventPublisher.OnAstralCopyShieldHeld += PutUpShield;
            m_eventPublisher.OnAstralCopyTeleportDown += UseTeleport; ;
        }

        private void CheckAttackButtonActivity()
        {
            isMeleeEnabledBeforeShield = PlayerInput.Instance.MeleeAttack.Enabled;
            isRangedEnabledBeforeShield = PlayerInput.Instance.RangedAttack.Enabled;
            isTeleportEnabledBeforeShield = PlayerInput.Instance.AstralCopyTeleport.Enabled;
        }

        private void DisableInputByShieldActivity()
        {
            PlayerInput.Instance.MeleeAttack.Disable();
            PlayerInput.Instance.RangedAttack.Disable();
            PlayerInput.Instance.AstralCopyTeleport.Disable();
            PlayerInput.Instance.Interact.Disable();
            PlayerInput.Instance.Jump.Disable();
            PlayerInput.Instance.Horizontal.Disable();
            PlayerInput.Instance.Vertical.Disable();
        }

        private void EnableInputByShieldInactivity()
        {
            if (isMeleeEnabledBeforeShield)
                PlayerInput.Instance.MeleeAttack.Enable();
            if (isRangedEnabledBeforeShield)
                PlayerInput.Instance.RangedAttack.Enable();
            if (isTeleportEnabledBeforeShield)
                PlayerInput.Instance.AstralCopyTeleport.Enable();

            PlayerInput.Instance.Interact.Enable();
            PlayerInput.Instance.Jump.Enable();
            PlayerInput.Instance.Horizontal.Enable();
            PlayerInput.Instance.Vertical.Enable();

            Debug.Log("Control enabled");
        }

        internal void PutUpShield(bool hasInput)
        {
            if (hasInput && !isShieldActive)
            {
                if (CurrentCopiesCount >= MaxAstralCopiesCount)
                    return;

                CreateAstralCopyShield();
                CurrentCopiesCount++;
                isShieldActive = true;
                CheckAttackButtonActivity();
                DisableInputByShieldActivity();
            }
            else if (!hasInput && isShieldActive)
            {
                DeactivateShield();
            }
        }

        private void CreateAstralCopyShield()
        {
            if (!m_currentCopies[AstralCopyMode.Shield])
                m_currentCopies[AstralCopyMode.Shield] = m_astralCopyPool.Pop();
            m_currentCopies[AstralCopyMode.Shield].GetComponent<SpriteRenderer>().flipX = m_eventPublisher.spriteRenderer.flipX;
            m_currentCopies[AstralCopyMode.Shield].GetComponent<SpriteRenderer>().color = AstralCopyColor;
            m_currentCopies[AstralCopyMode.Shield].transform.SetParent(transform);
            m_currentCopies[AstralCopyMode.Shield].transform.position = new Vector2(m_eventPublisher.transform.position.x + AstralCopyShieldSpawnDistance * m_eventPublisher.GetFacing(), m_eventPublisher.transform.position.y);
            m_currentCopies[AstralCopyMode.Shield].GetComponent<Animator>().SetTrigger("Defence");
            m_currentCopies[AstralCopyMode.Shield].GetComponent<AstralCopyView>().OnCollision += ShieldTakeDamage;
            m_currentCopies[AstralCopyMode.Shield].SetActive(true);
        }

        private void ShieldTakeDamage(GameObject shieldView)
        {
            if (m_currentCopies[AstralCopyMode.Shield].Equals(shieldView))
            {
                DeactivateShield();
                IsShieldCooldown = true;
                m_currentCopies[AstralCopyMode.Shield].GetComponent<AstralCopyView>().OnCollision -= ShieldTakeDamage;
                m_astralCopyPool.Push(m_currentCopies[AstralCopyMode.Shield]);
                m_currentCopies[AstralCopyMode.Shield] = null;
                StartCoroutine(ShieldCooldown());
            }
        }

        private void DeactivateShield()
        {
            m_currentCopies[AstralCopyMode.Shield].SetActive(false);
            isShieldActive = false;
            CurrentCopiesCount--;
            EnableInputByShieldInactivity();
        }

        IEnumerator ShieldCooldown()
        {
            yield return new WaitForSeconds(AstralCopyShieldCooldown);
            StopCoroutine("ShieldCooldown");
            IsShieldCooldown = false;
        }

        private void UseTeleport(bool obj)
        {
            if (!isTeleportActive)
            {
                if (CurrentCopiesCount >= MaxAstralCopiesCount)
                    return;

                CreateAstralCopyTeleport();
            }
            else
            {
                m_eventPublisher.transform.position = m_currentCopies[AstralCopyMode.Teleport].transform.position;
                m_astralCopyPool.Push(m_currentCopies[AstralCopyMode.Teleport]);
                m_currentCopies[AstralCopyMode.Teleport] = null;
                isTeleportActive = false;
                CurrentCopiesCount--;
            }
        }

        private void CreateAstralCopyTeleport()
        {
            if (!m_currentCopies[AstralCopyMode.Teleport])
                m_currentCopies[AstralCopyMode.Teleport] = m_astralCopyPool.Pop();

            m_currentCopies[AstralCopyMode.Teleport].transform.position = m_eventPublisher.transform.position;

            m_currentCopies[AstralCopyMode.Teleport].GetComponent<CapsuleCollider2D>().isTrigger = true;

            m_currentCopies[AstralCopyMode.Teleport].GetComponent<SpriteRenderer>().sprite= m_eventPublisher.spriteRenderer.sprite;
            m_currentCopies[AstralCopyMode.Teleport].GetComponent<SpriteRenderer>().flipX = m_eventPublisher.spriteRenderer.flipX;
            m_currentCopies[AstralCopyMode.Teleport].GetComponent<SpriteRenderer>().color = AstralCopyColor;

            m_currentCopies[AstralCopyMode.Teleport].GetComponent<Animator>().enabled = false;
            m_currentCopies[AstralCopyMode.Teleport].SetActive(true);
            CurrentCopiesCount++;
            isTeleportActive = true;
        }

        #endregion
    }
}
