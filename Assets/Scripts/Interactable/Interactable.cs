/// <author>Thomas Krahl</author>

using System;
using UnityEngine;
using eecon_lab.UI;
using UnityEngine.Events;

namespace eecon_lab.Interactables
{
    public class Interactable : MonoBehaviour
    {
        public static Action<bool, float> OnFocus;

        [SerializeField] protected bool isInteractable = true;
        [SerializeField, Range(0.1f, 10.0f)] protected float scanDuration = 2.0f;
        public UnityEvent OnScanComplete;
        protected bool onFocus;
        
        public bool IsInteractable => isInteractable;

        void Start()
        {
            if (!isInteractable) return;
            ScanUI.ScanComplete += ScanComplete;
            AdditionalStart();
        }

        private void OnDestroy()
        {
            ScanUI.ScanComplete -= ScanComplete;
        }

        protected virtual void AdditionalStart()
        {

        }

        public virtual void ChangeFocusState(bool focus)
        {      
            if (focus)
            {
                if (onFocus) return;
                OnFocus.Invoke(focus, scanDuration);
            }
            else
            {
                if (!onFocus) return;
                OnFocus.Invoke(focus, 0f);
            }

            onFocus = focus;
        }

        public virtual void ScanComplete()
        {
            onFocus = false;
            OnScanComplete?.Invoke();
        }
    }
}

