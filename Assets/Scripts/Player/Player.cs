/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab.Movement.MouseAndKeyboard;
using UnityEngine.SpatialTracking;
using Valve.VR.InteractionSystem;
using Valve.VR;

namespace eecon_lab.Character.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Camera activeCamera;
        [SerializeField] private PlayerMovement movementMK;

        [Header("VR")]
        [SerializeField] private Transform hmdTransform;
        [SerializeField] private TrackedPoseDriver.TrackingType trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
        [SerializeField] private Hand[] steamVrHands;

        private Transform trackingOriginTransform;
        private CharacterController characterController;
        private bool useVR;
        private float eyeHeight;

        public Camera ActiveCamera => activeCamera;

        public Hand leftHand
        {
            get
            {
                for (int j = 0; j < steamVrHands.Length; j++)
                {
                    if (!steamVrHands[j].gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    if (steamVrHands[j].handType != SteamVR_Input_Sources.LeftHand)
                    {
                        continue;
                    }

                    return steamVrHands[j];
                }

                return null;
            }
        }
        public Hand rightHand
        {
            get
            {
                for (int j = 0; j < steamVrHands.Length; j++)
                {
                    if (!steamVrHands[j].gameObject.activeInHierarchy)
                    {
                        continue;
                    }

                    if (steamVrHands[j].handType != SteamVR_Input_Sources.RightHand)
                    {
                        continue;
                    }

                    return steamVrHands[j];
                }

                return null;
            }
        }


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
                TrackedPoseDriver trackedPoseDriver = activeCamera.transform.parent.gameObject.AddComponent<TrackedPoseDriver>();
                trackedPoseDriver.trackingType = trackingType;
                trackingOriginTransform = transform;
                movementMK.isEnabled = false;
                characterController.enabled = false;
            }
            else
            {
                Debug.Log("<color=#A17FFF>USE Mouse and Keyboard</color>");
                movementMK.isEnabled = true;
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

