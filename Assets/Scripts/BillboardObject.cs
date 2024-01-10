/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab;

namespace TK
{
    public class BillboardObject : MonoBehaviour
    {
        [SerializeField, Tooltip("Leaving Empty uses the Player Camera if Player i set in Game Script")] private Camera referenceCamera;
        [SerializeField] private bool lockX;
        [SerializeField] private bool lockY;
        [SerializeField] private bool lockZ;
        private Transform lookAtCamera;
        private Vector3 startRotation;

        private void Start()
        {
            startRotation = transform.eulerAngles;
            if (referenceCamera != null)
            {
                lookAtCamera = referenceCamera.transform;
                return;
            }
            referenceCamera = Game.Instance.Player.ActiveCamera;
            lookAtCamera = referenceCamera.transform;
        }

        void LateUpdate()
        {
            if (lookAtCamera == null) return;
            transform.LookAt(lookAtCamera);

            Vector3 targetRotation = transform.eulerAngles;
            if (lockX) targetRotation.x = startRotation.x;
            if (lockY) targetRotation.y = startRotation.y;
            if (lockZ) targetRotation.z = startRotation.z;
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}

