/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace eecon_lab.UI
{
    public class Hud : MonoBehaviour
    {
        [Header("VR Settings")]
        [SerializeField] private float baseDistanceFromCamera = 2.0f;
        [SerializeField, Range(-1.0f, 1.0f)] private float distanceOffset = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        private Transform playerCamera;
        private Canvas canvas;
        private bool vrMode;

        [Header("Crosshair")]
        [SerializeField] private Vector2 crosshairSizeVR = new Vector2(40f,40f);
        [SerializeField] private Vector2 crosshairSizeMK = new Vector2(80f,80f);


        void Start ()
        {
            StartCoroutine(StartDelay());           
        }

        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(0.5f);           
            Setup();
        }

        private void LateUpdate()
        {
            if (!vrMode) return;
            float distance = baseDistanceFromCamera;

            Vector3 rayOrgin = playerCamera.position;
            Vector3 rayDirection = playerCamera.forward;
            Ray ray = new Ray(rayOrgin, rayDirection);

            if (Physics.Raycast(ray, out RaycastHit hit, baseDistanceFromCamera, groundLayer))
            {
                distance = Vector3.Distance(playerCamera.position, hit.point) - distanceOffset;
            }
           
            canvas.planeDistance = distance;
        }

        private void Setup()
        {
            vrMode = Game.Instance.VRactive;
            SetCanvasRenderMode(vrMode);            
            SetCrosshairSize(vrMode);
        }

        private void SetCanvasRenderMode(bool vrMode)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null) return;

            if (vrMode)
            {
                playerCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>().transform;
                canvas.planeDistance = baseDistanceFromCamera;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Game.Instance.PlayerGO.GetComponentInChildren<Camera>();
            }
            else
            {
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.worldCamera = null;
            }
        }

        private void SetCrosshairSize(bool vrMode)
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
                    rectTransform.sizeDelta = crosshairSizeVR;
                }
            }
            else
            {
                foreach (RectTransform rectTransform in imageTransforms)
                {
                    rectTransform.sizeDelta = crosshairSizeMK;
                }

                foreach (var image in images)
                {
                    image.material = null;
                }
            }
        }
    }
}

