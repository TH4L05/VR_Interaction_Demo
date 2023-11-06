/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace eecon_lab.Interactables
{
    public class MenuButtonInteractable : Interactable
    {
        [SerializeField] private Animator animator;
        public UnityEvent OnClick;
        private bool onHover;

        public override void ChangeFocusState(bool focus)
        {
            base.ChangeFocusState(focus);

            if(onHover && !onFocus)
            {
                onHover = false;
                animator.SetTrigger("Normal");
            }

            if(!onHover && onFocus) 
            {
                onHover = true;
                animator.SetTrigger("Highlighted");
            }          
        }

        public override void OnScanIsComplete()
        {
            base.OnScanIsComplete();
            if (!onHover) return;
            OnClick?.Invoke();
            animator.SetTrigger("Normal");

        }
    }
}

