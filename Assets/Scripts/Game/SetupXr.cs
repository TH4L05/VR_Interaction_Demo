/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

namespace eecon_lab
{
    public class SetupXr : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;
        [HideInInspector] public bool isInitialized;

        private bool initializeOnStart = true;
        private bool xrInitialized;

        public void Initialize()
        {          
            if(!xrInitialized)
            {
                if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);

                if(XRGeneralSettings.Instance.Manager.activeLoader != null && initializeOnStart)
                {
                    Debug.Log("XR already Initialized!");
                    xrInitialized = true;
                    isInitialized = xrInitialized;
                    return;
                }


                if (initializeOnStart)
                {
                    StartCoroutine(InitializeXR());
                }
            }
            else 
            {
                isInitialized = xrInitialized;
            }
        }

        private IEnumerator InitializeXR()
        {
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            Debug.Log("Start XR initialization ...");

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed.");
                xrInitialized = false;
            }
            else
            {
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                xrInitialized = true;
                Debug.Log("Initializing XR Success.");
            }

            isInitialized = xrInitialized;
        }

        public void StopXR()
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            xrInitialized =false;
            isInitialized = xrInitialized;
        }

    }
}

