/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR.Extras;

namespace eecon_lab
{
    public class PointerHandler : MonoBehaviour
    {
        [SerializeField] private SteamVR_LaserPointer pointer;

        private void Start()
        {
            pointer = GetComponent<SteamVR_LaserPointer>();
            pointer.PointerIn += PointerInside;
            pointer.PointerOut += PointerOutside;
            pointer.PointerClick += PointerClick;
        }

        private void PointerInside(object sender, PointerEventArgs e)
        {
            IPointerEnterHandler pointerEnterHandler = e.target.GetComponent<IPointerEnterHandler>();
            if (pointerEnterHandler == null)
            {
                return;
            }

            pointerEnterHandler.OnPointerEnter(new PointerEventData(EventSystem.current));
        }

        private void PointerOutside(object sender, PointerEventArgs e)
        {
            IPointerExitHandler pointerExitHandler = e.target.GetComponent<IPointerExitHandler>();
            if (pointerExitHandler == null)
            {
                return;
            }

            pointerExitHandler.OnPointerExit(new PointerEventData(EventSystem.current));

        }

        private void PointerClick(object sender, PointerEventArgs e)
        {
            IPointerClickHandler clickHandler = e.target.GetComponent<IPointerClickHandler>();
            if (clickHandler == null)
            {
                return;
            }

            clickHandler.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

}
