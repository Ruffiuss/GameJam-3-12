using UnityEngine;
using System.Collections.Generic;


namespace Gamekit2D
{
    internal sealed class AstralCopyPool
    {
        #region Fields

        private readonly Stack<GameObject> m_astralCopyViews = new Stack<GameObject>();
        private readonly GameObject m_originalPrefab;

        #endregion


        #region ClassLifeCycles

        internal AstralCopyPool()
        {
            m_originalPrefab = Resources.Load<GameObject>("AstralCopyView");
        }

        #endregion


        #region ClassLifeCycles

        ~AstralCopyPool()
        {
            foreach (var go in m_astralCopyViews)
            {
                go.GetComponent<AstralCopyView>().Unload();
            }
        }

        #endregion


        #region Methods

        internal void Push(GameObject go)
        {
            m_astralCopyViews.Push(go);
            go.SetActive(false);
        }

        internal GameObject Pop()
        {
            GameObject go;

            if (m_astralCopyViews.Count == 0)
            {
                go = Object.Instantiate(m_originalPrefab);
            }
            else go = m_astralCopyViews.Pop();
            go.SetActive(false);
            return go;
        }

        #endregion
    }
}
