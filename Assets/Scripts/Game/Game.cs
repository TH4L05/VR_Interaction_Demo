/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using eecon_lab.Input;
using TK;
using UnityEngine.XR.Management;
using eecon_lab.Character.Player;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace eecon_lab
{
    public class Game : MonoBehaviour
    {
        #region SerializedFields

        [Header("Option")]
        [SerializeField] private bool useVR = false;

        [Header("References")]
        [SerializeField] private SetupUnityXR xrSetup;
        [SerializeField] private Player player;
        [SerializeField] private Teleport teleport;
        [SerializeField] private SceneLoad sceneLoad;
        [SerializeField] private List<Canvas> UiCanvasList = new List<Canvas>();

        [Header("Dev")]
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
                sceneLoad.SetSceneIndex(1);
                sceneLoad.LoadScene();
            }
            if (Keyboard.current.f5Key.wasPressedThisFrame)
            {
                ingameLog.ChangeVisbilityState();
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
            if (testCamera != null) testCamera.SetActive(false);                      
            if (useVR)
            {
                SetupUnityXR.OnInitFinished += XRInitFinished;
                GameObject xr = GameObject.Find("XR_Setup");

                if (xr == null)
                {

                    //ShowIngameLogMessage("XR Setup Object is Missing !!", MessageType.Error);
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
            //ShowIngameLogMessage("Setup Done", MessageType.System);
            Debug.Log("Setup Done");
            VRactive = isInitialized;
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
        
        #endregion

        private IEnumerator StartDirector()
        {
            yield return new WaitForSeconds(logoStartDelay);
            if (logoPlayableDirector != null && logoPlayableDirector.playableAsset != null) logoPlayableDirector.Play();
        }

        /*public void ShowIngameLogMessage(string message, MessageType messageType)
        {
            if (ingameLog == null)
            {
                Debug.LogError("Cant Show Log Message !! -> Component Reference is Missing");
                return;
            }

            ingameLog.AddMessage(message, messageType);
        }*/
    }
}

