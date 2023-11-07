/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using eecon_lab.Input;
using TK;
using UnityEngine.XR.Management;
using eecon_lab.Character.Player;
using System.Collections.Generic;

namespace eecon_lab
{
    public class Game : MonoBehaviour
    {
        #region SerializedFields

        [Header("Option")]
        [SerializeField] private bool useVR = false;

        [Header("References")]
        [SerializeField] private SetupUnityXR xrSetup;
        [SerializeField] private GameObject playerVR_GameObject;
        [SerializeField] private GameObject playerMK_GameObject;
        [SerializeField] private Teleport teleport;
        [SerializeField] private SceneLoad sceneLoad;
        [SerializeField] private List<Canvas> UiCanvasList = new List<Canvas>();

        [Header("Dev")]
        [SerializeField] private GameObject testCamera;

        [Header("Logo")]
        [SerializeField] private bool showStartLogo = true;
        [SerializeField] private PlayableDirector logoPlayableDirector;
        [SerializeField] private float logoStartDelay = 2.0f;

        [Header("FadeIn")]
        [SerializeField] private bool fadeIn = true;
        //[SerializeField] private Color fadeColor = Color.black;
        [SerializeField] private PlayableDirector fadePlayableDirector;


        #endregion

        #region PublicFields

        public static Game Instance;
        public GameObject PlayerGO => activePlayer;
        public Teleport Teleport => teleport;
        public SceneLoad SceneLoader => sceneLoad;

        public bool VRactive {  get; private set; }

        #endregion

        #region PrivateFields

        private GameObject activePlayer;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;        
            Initialize();
        }
        
        void Start()
        {
            StartSetup();
        }

        private void LateUpdate()
        {
            if (InputHandler.Instance.ExtraInputValue1)
            {
                Debug.Log("Load Main Menu");
                sceneLoad.SetLevelIndex(1);
                sceneLoad.LoadAScene();
            }
        }

        private void OnDestroy()
        {
            SetupUnityXR.OnInitFinished -= XRInitFinished;
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            if(testCamera != null) testCamera.SetActive(false);
            SetupUnityXR.OnInitFinished += XRInitFinished;

            if (playerVR_GameObject != null) playerVR_GameObject.SetActive(false);
            if (playerMK_GameObject != null) playerMK_GameObject.SetActive(false);
            
            if (useVR)
            {
                Debug.Log("<color=#A17FFF>USE VR</color>");
                GameObject xr = GameObject.Find("XR_Setup");
                xrSetup = xr.GetComponent<SetupUnityXR>();
                if (xrSetup == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("SetupUnityXR Component is Missing!!");
                    UnityEditor.EditorApplication.isPlaying = false;
#endif                   
                    return;
                }
                xrSetup.Initialize();                             
            }
            else
            {
                Debug.Log("<color=#A17FFF>USE Mouse and Keyboard</color>");
                XRInitFinished(false);
            }
           
        }

        private void XRInitFinished(bool isInitialized)
        {           
            VRactive = isInitialized;
            SetPlayer(VRactive);
            UISetup();
        }
        
        private void StartSetup()
        {
            if (showStartLogo)  StartCoroutine("StartDirector");
            if (fadeIn && fadePlayableDirector != null) fadePlayableDirector.Play();
        }
        private void SetPlayer(bool useVRplayer)
        {
            if(useVRplayer)
            {
                activePlayer = playerVR_GameObject;   
                
            }
            else
            {
                activePlayer = playerMK_GameObject;
            }

            if (activePlayer == null)
            {
                Debug.LogError("Player GameObject Reference is Missing!!");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                return;
            }

            activePlayer.SetActive(true);
        }

        private void UISetup()
        {
            if (UiCanvasList.Count == 0) return;

            Camera camera = activePlayer.GetComponentInChildren<Camera>();

            foreach (var canvas in UiCanvasList)
            {
                if (VRactive)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = camera;
                }
                else
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.worldCamera = null;
                }
            }

        }
        
        #endregion

        private IEnumerator StartDirector()
        {
            yield return new WaitForSeconds(logoStartDelay);
            if (logoPlayableDirector != null && logoPlayableDirector.playableAsset != null) logoPlayableDirector.Play();
        }
    }
}

