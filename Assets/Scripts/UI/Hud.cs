/// <author>Thomas Krahl</author>

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace eecon_lab.UI
{
    public class Hud : MonoBehaviour
    {
        #region SerializedFields

        [Header("VR Settings")]
        [SerializeField] private float baseDistanceFromCamera = 2.0f;
        [SerializeField, Range(-1.0f, 1.0f)] private float distanceOffset = 0.1f;
        [SerializeField] private LayerMask groundLayer;
        

        [Header("Crosshair"), Space(2.0f)]
        [SerializeField] private GameObject crosshairParent;
        [SerializeField] private bool showCrosshair = true;
        [SerializeField] private Vector2 crosshairSizeVR = new Vector2(40f,40f);
        [SerializeField] private Vector2 crosshairSizeMK = new Vector2(80f,80f);

        [Header("FPS"), Space(2.0f)]
        [SerializeField] private bool dislpayFPS = false;
        [SerializeField] private TextMeshProUGUI fpsTextField;

        #endregion

        #region PrivateFields

        private Canvas canvas;
        private Transform playerCamera;
        private bool vrMode;
        private float dt;

        #endregion

        #region UnityFunctions

        void Start ()
        {
            StartCoroutine(StartDelay());           
        }


        private void LateUpdate()
        {
            if (dislpayFPS) UpdateFPS();


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

        #endregion

        #region Setup

        private void Setup()
        {
            vrMode = Game.Instance.VRactive;
            SetCanvasRenderMode(vrMode);            
            ShowCrosshair(showCrosshair);
            SetCrosshairSize(vrMode);
            ShowFPS(dislpayFPS);
        }
        
        private IEnumerator StartDelay()
        {
            yield return new WaitForSeconds(0.5f);           
            Setup();
        }

        #endregion

        #region Crosshair

        private void ShowCrosshair(bool show)
        {
            showCrosshair = show;
            if(crosshairParent != null) crosshairParent.SetActive(showCrosshair);
        }

        private void SetCanvasRenderMode(bool vrMode)
        {
            canvas = GetComponent<Canvas>();
            if (canvas == null) return;

            if (vrMode)
            {
                playerCamera = Game.Instance.Player.ActiveCamera.transform;
                canvas.planeDistance = baseDistanceFromCamera;
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.worldCamera = Game.Instance.Player.ActiveCamera;
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

        #endregion

        #region FPS

        public void ToggleFPS()
        {
            dislpayFPS = !dislpayFPS;
            ShowFPS(dislpayFPS);
        }

        public void ShowFPS(bool show)
        {
            dislpayFPS = show;
            if (fpsTextField != null) fpsTextField.gameObject.SetActive(dislpayFPS);
        }

        private void UpdateFPS()
        {
            if (!dislpayFPS) return;
            float frames = CalculateFPS();
            if (fpsTextField != null) fpsTextField.text = "FPS: " + Mathf.Ceil(frames).ToString();
        }

        private float CalculateFPS()
        {
            dt += (Time.deltaTime - dt) * 0.1f;
            float frames = 1.0f / dt;
            frames = Mathf.Clamp(frames, 0.0f, 999f);
            return frames;
        }

        #endregion
    }
}

