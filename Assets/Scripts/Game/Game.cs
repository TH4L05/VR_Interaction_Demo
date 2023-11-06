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
        [SerializeField] private bool useVR = false;

        [Header("References")]
        [SerializeField] private XRsetup xrSetup;
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
            GameObject xr = GameObject.Find("XR_Setup");
            xrSetup = xr.GetComponent<XRsetup>();

            playerVR_GameObject.SetActive(false);
            playerMK_GameObject.SetActive(false);

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
            if(testCamera != null) testCamera.SetActive(false);
        }

        private void SetPlayer(bool playerVR)
        {
            if(playerVR)
            {
                activePlayer = playerVR_GameObject;              
            }
            else
            {
                activePlayer = playerMK_GameObject;
            }

            activePlayer.SetActive(true);
        }
        
        private void StartSetup()
        {
            useVR = xrSetup.isInitialized;
            VRactive = useVR;
            SetPlayer(useVR);


            if (!showStartLogo) return;
            StartCoroutine("StartDirector");
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

