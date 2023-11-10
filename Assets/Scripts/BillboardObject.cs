/// <author>Thomas Krahl</author>

using UnityEngine;
using eecon_lab;

namespace TK
{
    public class BillboardObject : MonoBehaviour
    {
        private Transform playerCamera;

        private void Start()
        {
            //playerCamera = Camera.main.transform;
            playerCamera = Game.Instance.Player.ActiveCamera.transform;
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

