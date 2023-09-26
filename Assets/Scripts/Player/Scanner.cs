/// <author>Thomas Krahl</author>

using System;
using System.Collections;
using UnityEngine;
using eecon_lab.Interactables;
using eecon_lab.UI;

namespace eecon_lab.Character
{
    public class Scanner : MonoBehaviour
    {
        #region SerializedFields

        [SerializeField] private bool isEnabled;
        [SerializeField, Range(1f, 99f)] private float scanRange = 10.0f;
        [SerializeField] private Transform scanOrigin;
        [SerializeField] private LayerMask interactableLayer;

        #endregion

        #region PrivateFields

        private bool hasFocus;
        private Interactable interactableOnFocus;
        private bool paused;

        #endregion

        #region UnityFunctions

        void Start()
        {
            ScanUI.ScanComplete += OnScanComplete;
        }

        private void OnDestroy()
        {
            ScanUI.ScanComplete -= OnScanComplete;
        }

        void Update()
        {
            if(paused) return;  
            if(isEnabled) Scan();
        }

        #endregion

        #region Scan

        private void Scan()
        {
            if (paused) return;

            Vector3 rayOrigin = scanOrigin.position;
            Vector3 rayDirection = scanOrigin.forward;

            Ray ray = new Ray(rayOrigin, rayDirection);
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * scanRange, Color.red);

            if (Physics.Raycast(ray, out hit, scanRange, interactableLayer))
            {
                //Debug.Log("Scan");
                var interactable = hit.collider.GetComponent<Interactable>();

                if (interactable == null)
                {
                    Debug.Log("NoInteractable");
                    if(hasFocus)
                    {                       
                        hasFocus = false;
                        if(interactableOnFocus != null) interactableOnFocus.ChangeFocusState(true);
                    }                 
                    return;
                }

                if (!interactable.IsInteractable)
                {
                    Debug.Log("Interactable on focus is disabled");
                    return;
                }

                hasFocus = true;
                float distance = Vector3.Distance(transform.position, hit.point);
                Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

                interactableOnFocus = interactable;
                interactableOnFocus.ChangeFocusState(true);
            }
            else if(hasFocus)
            {
                hasFocus = false;
                if (interactableOnFocus != null) interactableOnFocus.ChangeFocusState(false);
                interactableOnFocus = null;                
            }
        }

        private void OnScanComplete()
        {
            paused = true;
            StartCoroutine(PauseWait1());
        }

        IEnumerator PauseWait1()
        {
            yield return new WaitForSeconds(0.25f);        
            if (interactableOnFocus != null) interactableOnFocus.ChangeFocusState(false);
            interactableOnFocus = null;
            StartCoroutine(PauseWait2());
        }

        IEnumerator PauseWait2()
        {          
            yield return new WaitForSeconds(2f);
            paused = false;
        }

        #endregion
    }
}

