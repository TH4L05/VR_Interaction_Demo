/// <author>Thomas Krahl</author>

using System.Collections.Generic;
using UnityEngine;
using TK;

namespace eecon_lab
{
    [CreateAssetMenu(fileName = "newGameConfig", menuName = "Data/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        #region SerializedFields

        [Header("Base")]
        public bool dontCreateConfigFile = false;
        public bool usePersistentDataPath = true;

        [Header("Default-Main")]
        public bool defaultUseVR= true;
        public bool defaultShowFPS = false;
        public bool defaultshowLog = false;

        [Header("Default-Audio")]
        public float defaultMasterVolume = 0.5f;
        public float defaultMusicVolume = 0.5f;
        public float defaultSfxVolume = 0.5f;

        [Header("Default-Graphic")]
        public bool defaultVsync = false;

        #endregion
        
        #region PrivateFields

        private bool useVR;
        private bool showFps;
        private bool showLog;
        private float masterVolume = 1f;
        private float musicVolume = 1f;
        private float sfxVolume = 1f;       
        private bool vSync;

        private string configname = "settings.cfg";
        
        #endregion

        #region PublicFields

        public bool UseVR => useVR;
        public bool ShowFps => showFps;
        public bool ShowLog => showLog;
        public float MasterVolume => masterVolume;
        public float MusicVolume => musicVolume;
        public float SfxVolume => sfxVolume;    
        public bool VSync => vSync;

        public bool loadDone = false;

        #endregion


        #region SetValues

        public void SetMasterVolume(float value)
        {
            masterVolume = value;
        }

        public void SetMusicVolume(float value)
        {
            musicVolume = value;
        }

        public void SetSFXVolume(float value)
        {
            sfxVolume = value;
        }

        #endregion

        #region LoadAndSave

        public void Start()
        {
            if (loadDone)
            {
                Debug.Log("Config Values Already loaded - Skip...");
                return;
            }

            if (dontCreateConfigFile)
            {
                Debug.Log("Skip Loading Config Values - Use Default Values");
                SetDefaults();
                loadDone = true;
                return;
            }

            if (Serialization.FileExistenceCheck(configname))
            {
                Debug.Log("Loading Config Values from File ...");
                LoadConfigValues();
                loadDone = true;
                return;
            }

            Debug.Log("ConfigFile Does not exist -> create new file with defaults");
            SetDefaults();
            SaveConfigValues();
            loadDone = true;
        }


        public void LoadConfigValues()
        {
            string file = configname;
            if (usePersistentDataPath)
            {
                file = Application.persistentDataPath + "\\" + configname;
            }

            List<string> content = Serialization.LoadTextByLine(file);
            string[] values = new string[content.Count];
            int index = 0;

            foreach (var item in content)
            {
                string[] temp = item.Split('=');             
                values[index] = temp[1];
                index++;
            }

            useVR = bool.Parse(values[0]);
            showFps = bool.Parse(values[1]);
            showLog = bool.Parse(values[2]);
            masterVolume = float.Parse(values[3]);
            musicVolume = float.Parse(values[4]);
            sfxVolume = float.Parse(values[5]);
        }

        public void SaveConfigValues()
        {
            string content = string.Empty;

            content += nameof(useVR) + "=" + useVR.ToString() + "\n";
            content += nameof(showFps) + "=" + showFps.ToString() + "\n";
            content += nameof(showLog) + "=" + showLog.ToString() + "\n";
            content += nameof(masterVolume) + "=" + masterVolume.ToString() + "\n";
            content += nameof(musicVolume) + "=" + musicVolume.ToString() + "\n";
            content += nameof(sfxVolume) + "=" + sfxVolume.ToString() + "\n";

            string file = configname;
            if( usePersistentDataPath )
            {
                file = Application.persistentDataPath + "\\" + configname;
            }
            Serialization.SaveText(content, file);
        }

        public void SetDefaults()
        {
            useVR = defaultUseVR;
            showFps = defaultShowFPS;
            showLog = defaultshowLog;
            masterVolume = defaultMasterVolume;
            musicVolume = defaultMusicVolume;
            sfxVolume = defaultSfxVolume;
            vSync = defaultVsync;
        }

        #endregion
    }
}

