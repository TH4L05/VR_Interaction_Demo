

using UnityEngine;

namespace eecon_lab.Character
{
    public class ToggleControllerVisibility : MonoBehaviour
    {
        [SerializeField] private bool showControllers = false;

        private void Update()
        {
            foreach (var hand in Valve.VR.InteractionSystem.Player.instance.hands)
            {
                if (showControllers)
                {
                    hand.ShowController();
                }
                else
                {
                    hand.HideController();
                }
            }
        }
    }
}

