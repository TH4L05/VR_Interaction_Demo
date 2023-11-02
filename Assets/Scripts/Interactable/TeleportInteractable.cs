/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace eecon_lab.Interactables
{
    public class TeleportInteractable : Interactable
    {     
        private Teleport teleport;

        protected override void AdditionalStart()
        {
            teleport = Game.Instance.Teleport;
        }

        public override void OnScanIsComplete()
        {
            if (!onFocus) return;
            teleport.SetTeleport(gameObject);
            base.OnScanIsComplete();
        }
    }
}

