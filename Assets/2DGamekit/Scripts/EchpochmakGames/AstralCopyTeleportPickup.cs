using UnityEngine;


namespace Gamekit2D
{
    public sealed class AstralCopyTeleportPickup : MonoBehaviour
    {
        public void ActivateAstralCopyTeleport()
        {
            PlayerInput.Instance.AstralCopyTeleport.Enable();
        }
    }
}
