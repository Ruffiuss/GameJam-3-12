using UnityEngine;


namespace Gamekit2D
{
    public sealed class AstralCopyController : MonoBehaviour
    {
        #region Properties

        private AstralCopyPool m_astralCopyPool;

        internal byte CurrentCopiesCount { get; }

        #endregion


        #region Fields

        public Color astralCopyColor = Color.blue;
        public float astralCopyShieldSpawnDistance = 1.0f;
        public float astralCopyShieldStrength = 1.0f;
        public float astralCopyShieldCooldown = 5.0f;
        public float astralCopySpearSpawnDistance = 3.0f;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            m_astralCopyPool = new AstralCopyPool();
            Initialize();
        }

        #endregion


        #region ClassLifeCycles



        #endregion


        #region Methods

        internal void Initialize()
        {
            var test = m_astralCopyPool.Pop();
            test.SetActive(true);
            test.GetComponent<SpriteRenderer>().color = astralCopyColor;
            test.transform.position = new Vector2(test.transform.position.x + astralCopyShieldSpawnDistance, transform.position.y);
        }

        #endregion
    }
}
