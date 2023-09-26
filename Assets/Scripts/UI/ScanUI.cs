/// <author>Thomas Krahl</author>

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using eecon_lab.Interactables;

namespace eecon_lab.UI
{
    public class ScanUI : MonoBehaviour
    {
        #region Events

        public static Action ScanComplete;

        #endregion

        #region SerializedFields

        [SerializeField] private Image scanFillImage;
        [SerializeField] private Image crosshair;
        [SerializeField] private Color crosshairColorDefault;
        [SerializeField] private Color crosshairColorFocus;

        #endregion

        #region PrivateFields

        private float currentScanTime;
        private float scanDuration;
        private bool onScan;

        #endregion

        #region UnityFunctions

        void Start ()
        {
            Interactable.OnFocus += UpdateScanBar;
            ResetScanbar();
        }

        private void OnDestroy()
        {
            Interactable.OnFocus -= UpdateScanBar;
        }

        private void LateUpdate()
        {
            if (!onScan) return;
            FillScanBar();
        }

        #endregion

        #region Scanbar

        private void UpdateScanBar(bool onFocus, float scanDuration)
        {       
            if(onFocus)
            {
                if (onScan) return;              
                this.scanDuration = scanDuration;
                scanFillImage.gameObject.SetActive(true);
                StartCoroutine(Delay());

            }
            else
            {
                if (!onScan) return;
                ResetScanbar();    
            }

            UpdateCrosshair(onFocus);
        }

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.5f);
            onScan = true;
        }

        private void FillScanBar()
        {            
            currentScanTime += Time.deltaTime;       
           
            if(currentScanTime >= scanDuration)
            {
                ScanComplete.Invoke();
                onScan = false;           
                ResetScanbar() ;
                UpdateCrosshair(false);
                return;
            }

            if (scanFillImage != null) scanFillImage.fillAmount = currentScanTime / scanDuration;
        }

        private void ResetScanbar()
        {
            onScan = false;
            if (scanFillImage != null) scanFillImage.fillAmount = 0.01f;
            scanDuration = 0f;
            currentScanTime = 0f;
            scanFillImage.gameObject.SetActive(false);           
        }

        #endregion

        #region Crosshair

        private void UpdateCrosshair(bool onFocus)
        {
            if (crosshair == null) return;

            if (onFocus)
            {
                crosshair.color = crosshairColorFocus;
            }
            else
            {
                crosshair.color = crosshairColorDefault;
            }
        }

        #endregion
    }
}

