/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eecon_lab
{
    [CreateAssetMenu(fileName = "NewGameData", menuName = "Data/GamedData")]
    public class GameData : ScriptableObject
    {
        public bool XrInitialized { get; set; }
        public bool XrInitializeFailed { get; set; }
    }
}

