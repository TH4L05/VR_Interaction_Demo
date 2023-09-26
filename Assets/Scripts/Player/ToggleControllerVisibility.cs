

using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace eecon_lab.Character
{
    public class ToggleControllerVisibility : MonoBehaviour
    {
        [SerializeField] private bool showControllers = false;
        //[SerializeField] private bool showHands = false;

        private void Start()
        {
            //StartCoroutine("HideHands");
        }

        private void Update()
        {
            foreach (var hand in Player.instance.hands)
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

        private IEnumerator HideHands()
        {
            yield return new WaitForSeconds(1.5f);
        }
    }
}

