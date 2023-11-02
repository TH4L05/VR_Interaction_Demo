/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using eecon_lab.Scene;
using eecon_lab.Interactables;

namespace eecon_lab.UI
{
    public class SceneSelectionButton : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private int sceneIndex;
        [SerializeField] private Sprite previewSprite;
        [SerializeField] private MenuButtonInteractable interactable;

        private SceneSelection sceneSelection;
        private TextMeshProUGUI buttonTextField;

        private void Start()
        {
            sceneSelection = transform.parent.GetComponent<SceneSelection>();
            buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
            interactable = GetComponent<MenuButtonInteractable>();
            //var button = GetComponent<Button>();
            SetText(interactable.IsInteractable);
        }

        public void ChangeLockedState(bool locked)
        {
            //var button = GetComponent<Button>();
            //button.interactable = locked;
            SetText(locked);
        }

        private void SetText(bool active)
        {
            if (active)
            {
                if (buttonTextField != null) buttonTextField.text = sceneName;
            }
            else
            {
                if (buttonTextField != null) buttonTextField.text = "- - -";
            }
        }

        public void SetPrevievImage()
        {
            if (sceneSelection == null) return;
            if (previewSprite == null) return;
            sceneSelection.SetScenePreview(sceneName, previewSprite, sceneIndex);
        }
    }
}

