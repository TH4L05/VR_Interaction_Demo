/// <author>Thomas Krahl</author>

using System;
using System.Collections;
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
                Debug.Log("<color=#00A513>Start Initialize XR ...</color>");
                if(XRGeneralSettings.Instance.Manager.activeLoader != null)
                {
                    Debug.Log("<color=#2AC93A>XR already Initialized!.</color>");
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
                Debug.Log("<color=#2AC93A>XR already Initialized!.</color>");
                OnInitFinished?.Invoke(xrInitialized);  
            }

            //Debug.Log("XR Active = " + XRSettings.isDeviceActive);                        
        }

        private IEnumerator InitializeXR()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            Debug.Log("<color=#00A513>Start XR initialization ...</color>");

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("<color=red>Initializing XR Failed.</color>");
                xrInitialized = false;
            }
            else
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                xrInitialized = true;
                Debug.Log("<color=#61C66B>Initializing XR Success.</color>");
            }

            OnInitFinished?.Invoke(xrInitialized);
            if(xrInitialized) InitializeSteamVR();
        }

        public void StopXR()
        {
            Debug.Log("<color=orange>Stopping XR ...</color>");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            xrInitialized =false;
            Debug.Log("<color=orange>XR stopped</color>");
        }

        public void InitializeSteamVR()
        {
            Debug.Log("<color=#45708E>InitSteamVR ...</color>");
            steamVR_Behaviour.InitializeSteamVR();
        }

    }
}

