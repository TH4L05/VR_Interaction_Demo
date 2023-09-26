/// <author>Thomas Krahl</author>

using UnityEngine;

namespace TK
{
    public class BillboardObject : MonoBehaviour
    {
        private Transform playerCamera;

        private void Start()
        {
            playerCamera = Camera.main.transform;
            //playerCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>().transform;
        }


        void LateUpdate()
        {
            if (playerCamera == null) return;
            transform.LookAt(playerCamera);

            Vector3 targetRotation = transform.eulerAngles;
            targetRotation.x = 0;
            targetRotation.z = 0;
            transform.rotation = Quaternion.Euler(targetRotation);
        }
    }
}

