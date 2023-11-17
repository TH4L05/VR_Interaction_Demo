/// <author>Thomas Krahl</author>

using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using Valve.VR;

namespace eecon_lab
{
    public class SetupUnityXR : MonoBehaviour
    {
        public static Action<bool> OnInitFinished;

        [SerializeField] private bool dontDestroyOnLoad;
        [SerializeField] private SteamVR_Behaviour steamVR_Behaviour;
        //public bool isInitialized;

        private bool initializeOnStart = true;
        private bool xrInitialized;

        private static SetupUnityXR instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
        }

        public void Initialize()
        {              
            if (!xrInitialized && initializeOnStart)
            {
                Game.Instance.ShowIngameLogMessage("Start Initialize XR...", MessageType.System);
                if (XRGeneralSettings.Instance.Manager.activeLoader != null)
                {
                    Game.Instance.ShowIngameLogMessage("XR already Initialized!", MessageType.Info);
                    StopXR();
                    StartCoroutine(InitializeXR());
                }
                else
                {
                    StartCoroutine(InitializeXR());
                }
            }
            else 
            {
                Game.Instance.ShowIngameLogMessage("XR already Initialized!", MessageType.Info);
                OnInitFinished?.Invoke(xrInitialized);  
            }                       
        }

        private IEnumerator InitializeXR()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            Game.Instance.ShowIngameLogMessage("Start XR initialization ...", MessageType.System);

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Game.Instance.ShowIngameLogMessage("Initializing XR Failed.", MessageType.Error);
                xrInitialized = false;
            }
            else
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                xrInitialized = true;
                Game.Instance.ShowIngameLogMessage("Initializing XR Success.", MessageType.System);
            }

            OnInitFinished?.Invoke(xrInitialized);
            if(xrInitialized) InitializeSteamVR();
        }

        public void StopXR()
        {
            Game.Instance.ShowIngameLogMessage("Stopping XR ...", MessageType.System);
            Debug.Log("<color=orange>Stopping XR ...</color>");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            xrInitialized =false;
            Game.Instance.ShowIngameLogMessage("XR stopped", MessageType.System);
        }

        public void InitializeSteamVR()
        {
            Game.Instance.ShowIngameLogMessage("InitSteamVR ...", MessageType.System);
            steamVR_Behaviour.InitializeSteamVR();
        }

    }
}

