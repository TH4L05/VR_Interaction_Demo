/// <author>Thomas Krahl</author>

using System;
using UnityEngine;
using eecon_lab.UI;
using UnityEngine.Events;

namespace eecon_lab.Interactables
{
    public class Interactable : MonoBehaviour
    {
        #region Events

        public static Action<float> OnFocus;
        public static Action LostFocus;

        [Header("Events"), Space(2f)]
        public UnityEvent OnFocusEnter;
        public UnityEvent OnFocusExit;
        public UnityEvent OnScanComplete;

        #endregion

        [Header("Settings"), Space(2f)]
        [SerializeField] protected bool isInteractable = true;
        [SerializeField, Range(0.1f, 10.0f)] protected float scanDuration = 2.0f;
        protected bool onFocus;
        
        public bool IsInteractable => isInteractable;

        void Start()
        {
            if (!isInteractable) return;
            ScanUI.ScanComplete += OnScanIsComplete;
            AdditionalStart();
        }

        private void OnDestroy()
        {
            ScanUI.ScanComplete -= OnScanIsComplete;
        }

        protected virtual void AdditionalStart()
        {
        }

        public virtual void ChangeFocusState(bool focus)
        {      
            if (focus)
            {
                if (onFocus) return;
                OnFocus?.Invoke(scanDuration);
                OnFocusEnter?.Invoke();
            }
            else
            {
                if (!onFocus) return;
                LostFocus?.Invoke();
                OnFocusExit?.Invoke();  
            }
            onFocus = focus;
        }

        public virtual void OnScanIsComplete()
        {
            if (!onFocus) return;
            onFocus = false;
            OnScanComplete?.Invoke();
            LostFocus?.Invoke();
        }
    }
}

