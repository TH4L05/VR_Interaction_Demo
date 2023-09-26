/// <author>Thomas Krahl</author>

using UnityEngine;


namespace eecon_lab.UI
{
    public class HudVR : MonoBehaviour
    {
        [SerializeField] private float baseDistance = 2.0f;
        [SerializeField] private LayerMask groundLayer;
        private Transform playerCamera;
        private Canvas canvas;


        void Start ()
        {
            canvas = GetComponent<Canvas>();
            playerCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>().transform;
            canvas.planeDistance = baseDistance;
        }

        private void LateUpdate()
        {
            float distance = baseDistance;

            Vector3 rayOrgin = playerCamera.position;
            Vector3 rayDirection = playerCamera.forward;
            Ray ray = new Ray(rayOrgin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, baseDistance, groundLayer))
            {
                distance = Vector3.Distance(playerCamera.position, hit.point) - 0.1f;
            }
           
            canvas.planeDistance = distance;
        }
    }
}

