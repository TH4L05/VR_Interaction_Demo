/// <author>Thomas Krahl</author>

using UnityEngine;

namespace eecon_lab.Interactables
{
    public enum InteractableType
    {
        Invalid = -1,
        Teleport,
        Button
    }

    [CreateAssetMenu(fileName = "newInteractableData", menuName ="Data/InteractableData")]
    public class InteractableData : ScriptableObject
    {
        [SerializeField] protected InteractableType type;
        [SerializeField, Range(0.1f, 10.0f)] protected float scanDuration = 2.0f;

        public InteractableType Type => type;
        public float ScanDuration => scanDuration;
    }
}

