/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TK;

namespace eecon_lab.Scene
{
    public class SceneSelection : MonoBehaviour
    {       
        [SerializeField] private Image scenePreview;
        [SerializeField] private TextMeshProUGUI scenePreviewText;
        [SerializeField] private Sprite scenePreviewSpriteDefault;
        //[SerializeField] private Button playButton;
        //[SerializeField] private TextMeshProUGUI playButtonText;
        private SceneLoad sceneLoad;

        private void Start()
        {
            sceneLoad = Game.Instance.SceneLoader;
        }

        public void SetScenePreview(string name, Sprite sprite, int sceneIndex)
        {
            scenePreview.sprite = sprite;
            scenePreviewText.text = name;
            SetSceneIndex(sceneIndex);
        }

        private void SetSceneIndex(int index)
        {
            if (sceneLoad == null || index == 0 || index < 0) return;
            sceneLoad.SetLevelIndex(index);
        }

        public void PlayScene()
        {
            sceneLoad.LoadAScene();
        }

        public void ResetScenePreview()
        {
            scenePreview.sprite = scenePreviewSpriteDefault;
            scenePreviewText.text = "";
        }

    }
}

