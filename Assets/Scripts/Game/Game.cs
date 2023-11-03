/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using eecon_lab.Input;
using TK;
using UnityEngine.XR.Management;

namespace eecon_lab
{
    public class Game : MonoBehaviour
    {
        #region SerializedFields

        [Header("Option")]
        [SerializeField] private bool initializeXR = false;

        [Header("References")]
        [SerializeField] private GameData gameData;
        [SerializeField] private GameObject playerVR_GameObject;
        [SerializeField] private GameObject playerMK_GameObject;
        [SerializeField] private Teleport teleport;
        [SerializeField] private SceneLoad sceneLoad;

        [Header("Dev")]
        [SerializeField] private GameObject testCamera;

        [Header("Logo")]
        [SerializeField] private bool showStartLogo = true;
        [SerializeField] private PlayableDirector logoPlayableDirector;
        [SerializeField] private float logoStartDelay = 2.0f;

        #endregion

        #region PublicFields

        public static Game Instance;
        public GameObject PlayerGO => activePlayer;
        public Teleport Teleport => teleport;
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
                sceneLoad.LoadSpecificScene(1);
            }
        }

        #endregion

        #region Setup
        
        private void Initialize()
        {
            if (testCamera != null) testCamera.SetActive(false);

            if (gameData.XrInitializeFailed)
            {
                SetPlayer(false);
                return;
            }

            if (initializeXR && !gameData.XrInitialized)
            {
                StartCoroutine(InitializeXR());             
            }
            else if(gameData.XrInitialized)
            {
                SetPlayer(true);
            }
            else
            {
                SetPlayer(false);
            }
        }

        private void SetPlayer(bool playerVR)
        {
            if(playerVR)
            {
                activePlayer = playerVR_GameObject;
                VRactive = true;
            }
            else
            {
                activePlayer = playerMK_GameObject;
                VRactive = false;
            }

            activePlayer.SetActive(true);
        }
        
        private void StartSetup()
        {
            if (!showStartLogo) return;
            StartCoroutine("StartDirector");
        }
        
        private IEnumerator InitializeXR()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed.");              
                gameData.XrInitialized = false;
                gameData.XrInitializeFailed = true;
                SetPlayer(false);
            }
            else
            {            
                XRGeneralSettings.Instance.Manager.StartSubsystems();                   
                gameData.XrInitialized = true;
                gameData.XrInitializeFailed = false;
                SetPlayer(true);
            }
        }
        
        public void StopXR()
        {          
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }

        #endregion

        private IEnumerator StartDirector()
        {
            yield return new WaitForSeconds(logoStartDelay);
            if (logoPlayableDirector != null && logoPlayableDirector.playableAsset != null) logoPlayableDirector.Play();
        }
    }
}

