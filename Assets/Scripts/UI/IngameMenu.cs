/// <author>Thomas Krahl</author>

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using Valve.VR;

public class IngameMenu : MonoBehaviour
{
    #region Events

    [SerializeField] private UnityEvent OnMenuOpen;
    [SerializeField] private UnityEvent OnMenuClose;

    #endregion

    #region Fields

    [Header("General")]
    public static bool GamePaused;
    private bool pauseMenuActive;
    [SerializeField] private SteamVR_Action_Boolean toogleMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionMenu;
  
    [Header("Playables")]
    [SerializeField] private PlayableDirector showPauseMenu;
    [SerializeField] private PlayableDirector hidePauseMenu;
    [SerializeField] private PlayableDirector showOptionsMenu;
    [SerializeField] private PlayableDirector hideOptionsMenu;

    #endregion

    #region UnityFunctions

    public void Awake()
    {
        GamePaused = false;
        if(pauseMenu != null ) pauseMenu.SetActive(false);
        /*var options = optionMenu.GetComponent<OptionsMenu>();
        if (!options) return;
        options.Setup();
        options.SetSensitivityInPlayer();*/
    }

    private void Start()
    {
        toogleMenu.onStateDown += ToogleMenu_onStateDown;
    }

    private void ToogleMenu_onStateDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        ToggleMenu();
    }

    private void LateUpdate()
    {
    }

    #endregion

    #region PauseMenu

    public void ToggleMenu()
    {
        //if (GamePaused && !pauseMenuActive)
        if (GamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void ToggleOptionMenu(bool active)
    {
        pauseMenuActive = active;
        optionMenu.SetActive(active);
        pauseMenu.SetActive(!active);
    }

    public void Pause()
    {
        GamePaused = true;
        pauseMenuActive = true;
        //ChangeCursorVisibility(true);
        //showPauseMenu?.Play();
        OnMenuOpen?.Invoke();
        if (pauseMenu != null) pauseMenu.SetActive(true);
        SetTimeScale(0f);
        //Debug.Log(GamePaused);
    }

    public void Resume()
    {
        GamePaused = false;
        pauseMenuActive = false;
        SetTimeScale(1f);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        //hidePauseMenu?.Play();
        OnMenuClose?.Invoke();
        //ChangeCursorVisibility(false);
        //Debug.Log(GamePaused);
    }

    public void ChangeCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        //Debug.Log(Time.timeScale);
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
    
#endif
    }

    #endregion

    /*#region Audio

    public void Play_Button_Click_Audio()
    {     
    }

    public void Play_Button_Hover_Audio()
    {
    }

    public void PlayStopMusicDampAudio()
    {
    }


    #endregion Audio*/
}
