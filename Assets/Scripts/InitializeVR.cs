

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Management;

public class InitializeVR : MonoBehaviour
{
    public static bool XR = false;

    public void InitializeXR()
    {
        StartCoroutine(StartXR());
    }


    public IEnumerator StartXR()
    {
        //Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            //Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            XR = false;
        }
        else
        {
            //Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            XR = true;
        }
    }

    public void StopXR()
    {
        //Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        //Debug.Log("XR stopped completely.");
    }
}
