/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace eecon_lab.Character.Player
{
    public class Player : MonoBehaviour
    {
        [Header("VR")]
        [SerializeField] private Transform hmdTransform;
        

        private Transform trackingOriginTransform;
        private bool useVR;
        public float eyeHeight;


        public void Start()
        {
            trackingOriginTransform = transform;
            useVR = Game.Instance.VRactive;
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

