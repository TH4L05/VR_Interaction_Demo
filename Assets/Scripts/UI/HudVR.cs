/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;

namespace eecon_lab.UI
{
    public class HudVR : MonoBehaviour
    {
        [SerializeField] private float baseDistance = 2.0f;
        [SerializeField, Range(-1.0f, 1.0f)] private float offset = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        private Transform playerCamera;
        private Canvas canvas;
        private bool vrMode;


        void Start ()
        {
            vrMode = Game.Instance.VRactive;
            Setup();
            
        }

        private void LateUpdate()
        {
            if (!vrMode) return;
            float distance = baseDistance;

            Vector3 rayOrgin = playerCamera.position;
            Vector3 rayDirection = playerCamera.forward;
            Ray ray = new Ray(rayOrgin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, baseDistance, groundLayer))
            {
                distance = Vector3.Distance(playerCamera.position, hit.point) - offset;
            }
           
            canvas.planeDistance = distance;
        }

        private void Setup()
        {
            canvas = GetComponent<Canvas>();
            if (vrMode)
            {            
                playerCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>().transform;
                canvas.planeDistance = baseDistance;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>();
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }
            SetCrosshairSize();
        }

        private void SetCrosshairSize()
        {
            Image[] images = GetComponentsInChildren<Image>(true);
            RectTransform[] imageTransforms = new RectTransform[images.Length];

            for (int i = 0; i < images.Length; i++)
            {
                imageTransforms[i] = images[i].rectTransform;
            }
       
            if(vrMode)
            {
                foreach (RectTransform rectTransform in imageTransforms)
                {
                    rectTransform.sizeDelta = new Vector2 (40f, 40f);
                }
            }
            else
            {
                foreach (RectTransform rectTransform in imageTransforms)
                {
                    rectTransform.sizeDelta = new Vector2(80f, 80f);
                }

                foreach (var image in images)
                {
                    image.material = null;
                }
            }

            
        }
    }
}

