/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;

namespace TK
{
    public class Trigger : MonoBehaviour
    {
        public UnityEvent OnTriggerEnterEvent;
        public UnityEvent OnTriggerExitEvent;
        [SerializeField] private bool active;

        private void OnTriggerEnter(Collider collider)
        {
            if (active)
            {
                OnTriggerEnterEvent?.Invoke();
            }

        }

        private void OnTriggerStay(Collider ocollider)
        {

        }

        private void OnTriggerExit(Collider ocollider)
        {
            if (active)
            {
                OnTriggerExitEvent?.Invoke();
            }
        }
    }
}

