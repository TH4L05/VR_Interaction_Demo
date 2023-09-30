/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace eecon_lab.Interactables
{
    public class MenuButtonInteractable : Interactable
    {     
        [SerializeField] private UnityEvent OnHoverEnter;
        [SerializeField] private UnityEvent OnHoverExit;
        private bool onHover;
        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
        }

        public override void ChangeFocusState(bool focus)
        {
            base.ChangeFocusState(focus);

            if(onHover && !onFocus)
            {
                onHover = false;
                PointerEventData e = new PointerEventData(EventSystem.current);
                button.OnPointerExit(e);
                OnFocus?.Invoke(onFocus, 0f);
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

        public override void ScanComplete()
        {
            base.ScanComplete();
            Debug.Log("Scan Complete");
            if (!onHover) return;
            button.onClick.Invoke();
            BaseEventData eventData = new BaseEventData(EventSystem.current);
            button.OnDeselect(eventData);
        }
    }
}

