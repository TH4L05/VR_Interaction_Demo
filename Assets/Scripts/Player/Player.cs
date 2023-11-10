/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab.Movement.MouseAndKeyboard;

namespace eecon_lab.Character.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Camera cameraVR;
        [SerializeField] private Camera cameraMK;
        [SerializeField] private PlayerMovementMK movementMK;

        [Header("VR")]
        [SerializeField] private Transform hmdTransform;
        

        private Transform trackingOriginTransform;
        private CharacterController characterController;
        private bool useVR;
        private float eyeHeight;
        private Camera activeCamera;

        public Camera ActiveCamera => activeCamera;

        public void Start()
        {                     
        }

        public void Setup(bool vrActive)
        {
            characterController = GetComponent<CharacterController>();
            useVR = vrActive;

            if (useVR)
            {
                Debug.Log("<color=#A17FFF>USE VR</color>");
                trackingOriginTransform = transform;
                movementMK.isEnabled = false;
                characterController.enabled = false;
                cameraVR.gameObject.SetActive(true);
                cameraMK.gameObject.SetActive(false);
                activeCamera = cameraVR;
            }
            else
            {
                Debug.Log("<color=#A17FFF>USE Mouse and Keyboard</color>");
                movementMK.isEnabled = true;
                cameraMK.gameObject.SetActive(true);
                cameraVR.gameObject.SetActive(false);
                activeCamera = cameraMK;
            }
        }

        private void Update()
        {
            if (useVR) GetEyeHeight();     
        }

        private void GetEyeHeight()
        {
            Transform hmd = hmdTransform;
            if (hmd)
            {
                Vector3 eyeOffset = Vector3.Project(hmd.position - trackingOriginTransform.position, trackingOriginTransform.up);
                eyeHeight = eyeOffset.magnitude / trackingOriginTransform.lossyScale.x;
                return;
            }
            eyeHeight = 0.0f;
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 position = new Vector3(hmdTransform.position.x, eyeHeight, hmdTransform.position.z);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, 0.20f);
        }

    }
}

