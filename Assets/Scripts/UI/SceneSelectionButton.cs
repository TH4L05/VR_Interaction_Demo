/// <author>Thomas Krahl</author>

using UnityEngine;
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
               
        private MenuButtonInteractable interactable;
        private SceneSelection sceneSelection;
        private TextMeshProUGUI buttonTextField;

        private void Start()
        {
            sceneSelection = transform.parent.GetComponent<SceneSelection>();
            buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
            interactable = GetComponent<MenuButtonInteractable>();
            SetText(interactable.IsInteractable);
        }

        public void ChangeLockedState(bool locked)
        {
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

        public void UpdatePreview()
        {
            if (sceneSelection == null) return;
            sceneSelection.SetScenePreview(sceneName, previewSprite, sceneIndex);
        }
    }
}

