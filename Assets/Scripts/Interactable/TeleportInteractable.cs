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

        public override void OnScanComplete()
        {
            if(!onFocus)
            {
                return;
            }
            Debug.Log("ScanComplete");
            base.OnScanComplete();
            onFocus = false;
            teleport.SetTeleport(gameObject);
            OnScanCompleteEvent?.Invoke();  
        }
    }
}

