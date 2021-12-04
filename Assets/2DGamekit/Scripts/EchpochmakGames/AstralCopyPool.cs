using UnityEngine;
using System.Collections.Generic;


namespace Gamekit2D
{
    internal sealed class AstralCopyPool
    {
        #region Fields

        private readonly Stack<GameObject> _astralCopyViews = new Stack<GameObject>();
        private readonly GameObject _originalPrefab;

        #endregion


        #region ClassLifeCycles

        internal AstralCopyPool()
        {
            _originalPrefab = Resources.Load<GameObject>("AstralCopyView");
        }

        #endregion


        #region Methods

        internal void Push(GameObject go)
        {
            _astralCopyViews.Push(go);
            go.SetActive(false);
        }

        internal GameObject Pop()
        {
            GameObject go;

            if (_astralCopyViews.Count == 0)
            {
                go = Object.Instantiate(_originalPrefab);
            }
            else go = _astralCopyViews.Pop();
            return go;
        }

        #endregion
    }
}
