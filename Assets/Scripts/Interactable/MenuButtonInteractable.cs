/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace eecon_lab.Interactables
{
    public class MenuButtonInteractable : Interactable
    {
        public UnityEvent OnScanCompleteEvent;

        [SerializeField] private Button button;
        public UnityEvent OnHoverEnter;
        public UnityEvent OnHoverExit;
        public bool onHover;

        public override void ChangeFocusState(bool focus)
        {
            base.ChangeFocusState(focus);

            if(onHover && !onFocus)
            {
                onHover = false;
                PointerEventData e = new PointerEventData(EventSystem.current);
                button.OnPointerExit(e);
                OnFocus?.Invoke(onFocus, -1f);
                OnHoverExit?.Invoke();

                BaseEventData eventData = new BaseEventData(EventSystem.current);
                button.OnDeselect(eventData);
            }

            if(!onHover && onFocus) 
            {
                onHover = true;
                PointerEventData e = new PointerEventData(EventSystem.current);
                button.OnPointerEnter(e);
                OnFocus?.Invoke(onFocus, scanDuration);
                OnHoverEnter?.Invoke();
            }          
        }

        public override void OnScanComplete()
        {
            base.OnScanComplete();
            Debug.Log("Scan Complete");
            if (!onHover) return;
            button.onClick.Invoke();
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            button.OnDeselect(eventData);
        }
    }
}

