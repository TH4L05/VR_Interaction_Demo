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
        private SceneLoad sceneLoader;

        private void Start()
        {
            sceneLoader = Game.Instance.SceneLoader;
            SetScenePreviewDefault();
        }

        public void SetScenePreview(string name, Sprite sprite, int sceneIndex)
        {
            scenePreview.sprite = sprite;
            scenePreviewText.text = name;
            SetSceneIndex(sceneIndex);
        }

        private void SetSceneIndex(int index)
        {
            if (sceneLoader == null || index == 0 || index < 0) return;
            sceneLoader.SetSceneIndex(index);
        }

        public void LoadScene()
        {
            sceneLoader.LoadScene();
        }

        public void SetScenePreviewDefault()
        {
            scenePreview.sprite = scenePreviewSpriteDefault;
            scenePreviewText.text = "";
        }

    }
}

