using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.XR.Management;
using Unity.VisualScripting;

namespace eecon_lab
{
    public class XRsetup : MonoBehaviour
    {
        [SerializeField] private bool initializeOnStart;
        [SerializeField] private bool dontDestroyOnLoad;
        private bool xrInitialized;

        public bool isInitialized;

        private void Awake()
        {
            Initialize();
        }


        private void Initialize()
        {          
            if(!xrInitialized)
            {
                DontDestroyOnLoad(gameObject);

                if(XRGeneralSettings.Instance.Manager.activeLoader != null && initializeOnStart)
                {
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

    }
}

