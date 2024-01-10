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
        [SerializeField] private Image crosshairOuter;
        [SerializeField] private Sprite crosshairOuterDefault;
        [SerializeField] private Sprite crosshairOuterFocus;
        //[SerializeField] private Color colorDefault;
        //[SerializeField] private Color colorOnFocus;

        #endregion

        #region PrivateFields

        private float currentScanTime;
        private float scanDuration;
        private bool onScan;

        #endregion

        #region UnityFunctions

        void OnEnable ()
        {
            Interactable.OnFocus += OnFocus;
            Interactable.LostFocus += LostFocus;
            ResetScanbar();
        }

        private void OnDestroy()
        {
            Interactable.OnFocus -= OnFocus;
            Interactable.LostFocus -= LostFocus;
        }

        #endregion

        #region Focus
        private void OnFocus( float scanDuration)
        {
            UpdateCrosshair(true);
            if (onScan) return;
            this.scanDuration = scanDuration;
            scanFillImage.gameObject.SetActive(true);
            StartCoroutine(FillScanbar());
            onScan = true;
        }
        
        private void LostFocus()
        {
            UpdateCrosshair(false);
            if (!onScan) return;
            onScan = false;
            StopCoroutine(FillScanbar());
            ResetScanbar();
        }

        #endregion

        #region Scanbar

        IEnumerator FillScanbar()
        {
            yield return new WaitForSeconds(0.5f);
            while (currentScanTime < scanDuration)
            {
                UpdateScanBar();
                currentScanTime += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
            if (onScan) ScanComplete.Invoke();
            ResetScanbar();
        }

        private void UpdateScanBar()
        {            
            if (scanFillImage != null) scanFillImage.fillAmount = currentScanTime / scanDuration;
        }

        private void ResetScanbar()
        {
           
            if (scanFillImage != null) scanFillImage.fillAmount = 0.01f;
            scanDuration = 0f;
            currentScanTime = 0f;
            scanFillImage.gameObject.SetActive(false);           
        }

        #endregion

        #region Crosshair

        private void UpdateCrosshair(bool focus)
        {
            if (crosshairOuter == null) return;

            if (focus)
            {
                crosshairOuter.sprite = crosshairOuterFocus;
                //crosshairOuter.color = colorOnFocus;
                //scanFillImage.color = colorOnFocus;
            }
            else
            {
                crosshairOuter.sprite = crosshairOuterDefault;
                //crosshairOuter.color = colorDefault;
                //scanFillImage.color = colorDefault;
            }
        }

        #endregion
    }
}

