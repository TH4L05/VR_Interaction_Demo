/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using eecon_lab.Scene;

namespace eecon_lab.UI
{
    [System.Serializable]
    public struct SceneSelectionData
    {
        public string sceneName;
        public int sceneIndex;
        public Sprite previewSprite;
    }

    public class SceneSelectionButton : MonoBehaviour
    {
        #region Fields

        [SerializeField] private SceneSelectionData sceneSelectionData;

        private SceneSelection sceneSelection;
        private TextMeshProUGUI buttonTextField;

        #endregion

        #region UnityFunctions

        private void Start()
        {
            sceneSelection = transform.parent.GetComponent<SceneSelection>();
            buttonTextField = GetComponentInChildren<TextMeshProUGUI>();
            var button = GetComponent<Button>();
            SetText(button.interactable);
        }

        #endregion;

        public void ChangeLockedState(bool locked)
        {
            var button = GetComponent<Button>();
            button.interactable = locked;
            SetText(!locked);
        }

        private void SetText(bool active)
        {
            if (active)
            {
                if (buttonTextField != null) buttonTextField.text = sceneSelectionData.sceneName;
            }
            else
            {
                if (buttonTextField != null) buttonTextField.text = "- - -";
            }
        }

        public void SetPrevievImage()
        {
            if (sceneSelection == null) return;
            if (sceneSelectionData.previewSprite == null) return;
            sceneSelection.SetScenePreview(sceneSelectionData.sceneName, sceneSelectionData.previewSprite, sceneSelectionData.sceneIndex);
        }
    }
}

