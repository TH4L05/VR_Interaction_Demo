/// <author>Thomas Krahl</author>

using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using eecon_lab.Input;
using TK;

namespace eecon_lab
{
    public class Game : MonoBehaviour
    {
        #region SerializedFields

        [Header("References")]
        [SerializeField] private GameObject playerGameObject;
        [SerializeField] private Teleport teleport;
        [SerializeField] private SceneLoad sceneLoad;

        [Header("Test")]
        [SerializeField] private GameObject testCamera;

        [Header("Logo")]
        [SerializeField] private bool showStartLogo = true;
        [SerializeField] private PlayableDirector logoPlayableDirector;
        [SerializeField] private float logoStartDelay = 2.0f;

        #endregion

        #region PublicFields

        public static Game Instance;
        public GameObject PlayerGO => playerGameObject;
        public Teleport Teleport => teleport;

        #endregion

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            if(testCamera != null) testCamera.SetActive(false);
        }

        private void LateUpdate()
        {
            if (InputHandler.Instance.ExtraInputValue1)
            {
                sceneLoad.LoadSpecificScene(1);
            }
        }

        void Start()
        {
            if (!showStartLogo) return;
            StartCoroutine("StartDirector");
            //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            //XRGeneralSettings.Instance.Manager.StartSubsystems();
        }

        #endregion

        private IEnumerator StartDirector()
        {
            yield return new WaitForSeconds(logoStartDelay);
            if (logoPlayableDirector != null && logoPlayableDirector.playableAsset != null) logoPlayableDirector.Play();
        }
    }
}

