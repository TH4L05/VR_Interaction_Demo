/// <author>Thomas Krahl</author>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using eecon_lab.Input;
using eecon_lab.Character.Player;
using eecon_lab.UI;
using TK;

namespace eecon_lab
{
    public class Game : MonoBehaviour
    {
        #region SerializedFields

        [Header("References")]
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private SetupUnityXR xrSetup;
        [SerializeField] private Player player;
        [SerializeField] private Teleport teleport;
        [SerializeField] private SceneLoad sceneLoad;
        [SerializeField] private Hud hud;
        [SerializeField] private List<Canvas> UiCanvasList = new List<Canvas>();

        [Header("Dev")]
        [SerializeField] private bool fkKeysEnabled = true;
        [SerializeField] private GameObject testCamera;
        [SerializeField] private IngameLog ingameLog;

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
        public Player Player => player;
        public Teleport Teleport => teleport;
        public SceneLoad SceneLoader => sceneLoad;
        public bool VRactive {  get; private set; }

        #endregion

        #region PrivateFields
        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;                      
        }
        
        void Start()
        {
            Initialize();
            StartSetup();
        }

        private void LateUpdate()
        {
            if (!fkKeysEnabled) return;
            DevInputsCheck();
        }

        private void OnDestroy()
        {
            SetupUnityXR.OnInitFinished -= XRInitFinished;
        }

        private void OnApplicationQuit()
        {
            gameConfig.loadDone = false;
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            Application.targetFrameRate = 90;
            if (testCamera != null) testCamera.SetActive(false);
            gameConfig.Start();
            if (gameConfig.UseVR)
            {
                SetupUnityXR.OnInitFinished += XRInitFinished;
                GameObject xr = GameObject.Find("XR_Setup");

                if (xr == null)
                {
                    Debug.LogError("XR Setup Object is Missing !!");
                    XRInitFinished(false);
                    return;
                }

                xrSetup = xr.GetComponent<SetupUnityXR>();
                xrSetup.Initialize();                             
            }
            else
            {
                XRInitFinished(false);
            }          
        }

        private void XRInitFinished(bool isInitialized)
        {         
            Debug.Log("XR Setup Done");
            VRactive = isInitialized;
            AudioSetup();
            PlayerSetup(VRactive);
            UISetup();
        }
        
        private void StartSetup()
        {
            if (showStartLogo)  StartCoroutine("StartDirector");
            if (fadeIn && fadePlayableDirector != null) fadePlayableDirector.Play();
        }

        private void PlayerSetup(bool vrActive)
        {           
            if (player == null) return;
            player.Setup(vrActive);
        }

        private void UISetup()
        {
            if (hud != null) hud.ShowFPS(gameConfig.ShowFps);
            if (ingameLog != null) ingameLog.ShowLog(gameConfig.ShowLog);

            if (UiCanvasList.Count == 0) return;

            Camera camera = player.ActiveCamera;
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

        private void AudioSetup()
        {
        }
        
        #endregion

        private IEnumerator StartDirector()
        {
            yield return new WaitForSeconds(logoStartDelay);
            if (logoPlayableDirector != null && logoPlayableDirector.playableAsset != null) logoPlayableDirector.Play();
        }

        private void DevInputsCheck()
        {
            if (Keyboard.current.f1Key.wasPressedThisFrame)
            {
                Debug.Log("Load Main Menu");
                sceneLoad.SetSceneIndex(0);
                sceneLoad.LoadScene();
            }
            if (Keyboard.current.f2Key.wasPressedThisFrame)
            {
                hud.ToggleFPS();
            }
            if (Keyboard.current.f3Key.wasPressedThisFrame)
            {
                ingameLog.ChangeVisbilityState();
            }
            if (Keyboard.current.f4Key.wasPressedThisFrame)
            {
                Application.Quit();
            }
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                sceneLoad.ReloadCurrentScene();
            }
        }
    }
}

