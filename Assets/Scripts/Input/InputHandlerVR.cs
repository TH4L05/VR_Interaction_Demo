/// <author>Thomas Krahl</author>

using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

namespace eecon_lab.Input
{
    public class InputHandlerVR : MonoBehaviour
    {
        [SerializeField] private SteamVR_Action_Boolean teleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");
        //[SerializeField] private SteamVR_Action_Boolean interactUIAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");
        [SerializeField] private SteamVR_LaserPointer pointer;

        private void Start()
        {
            teleportAction.onStateDown += TeleportAction_onStateDown;
            teleportAction.onStateUp += TeleportAction_onStateUp;

            //interactUIAction.onStateDown += InteractUIAction_onStateDown;
            //interactUIAction.onStateUp += InteractUIAction_onStateUp;
        }

        private void OnDestroy()
        {
            teleportAction.onStateDown -= TeleportAction_onStateDown;
            teleportAction.onStateUp -= TeleportAction_onStateUp;
            //interactUIAction.onStateDown -= InteractUIAction_onStateDown;
            //interactUIAction.onStateUp -= InteractUIAction_onStateUp;
        }

        private void TeleportAction_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("NOT_ACTIVE");
            pointer.gameObject.SetActive(true);
        }

        private void TeleportAction_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            Debug.Log("ACTIVE");
            pointer.gameObject.SetActive(false);
        }

        private void InteractUIAction_onStateUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            pointer.gameObject.SetActive(false);
        }

        private void InteractUIAction_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            pointer.gameObject.SetActive(true);
        }
    }
}

