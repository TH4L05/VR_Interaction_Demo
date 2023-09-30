/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace eecon_lab.Interactables
{
    public class TeleportInteractable : Interactable
    {
        public UnityEvent OnScanCompleteEvent;
        private Teleport teleport;

        protected override void AdditionalStart()
        {
            teleport = Game.Instance.Teleport;
        }

        public override void ScanComplete()
        {
            if(!onFocus)
            {
                return;
            }
            Debug.Log("ScanComplete");
            base.ScanComplete();
            onFocus = false;
            teleport.SetTeleport(gameObject);
            OnScanCompleteEvent?.Invoke();  
        }
    }
}

